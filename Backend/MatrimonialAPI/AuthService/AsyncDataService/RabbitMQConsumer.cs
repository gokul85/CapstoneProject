using AuthService.Interfaces;
using AuthService.Models.DTOs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace AuthService.AsyncDataService
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly string _profileQueueName;
        private readonly string _paymentQueueName;
        private IConnection _connection;
        private IModel _profileChannel;
        private IModel _paymentChannel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _profileQueueName = configuration["RabbitMQ:ProfileQueueName"];
            _paymentQueueName = configuration["RabbitMQ:PaymentQueueName"];
            CreateConnection(configuration);
            _scopeFactory = scopeFactory;
        }

        private void CreateConnection(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:Host"], UserName = configuration["RabbitMQ:UserName"], Password = configuration["RabbitMQ:Password"] };
            _connection = factory.CreateConnection();
            _profileChannel = _connection.CreateModel();
            _profileChannel.QueueDeclare(queue: _profileQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _paymentChannel = _connection.CreateModel();
            _paymentChannel.QueueDeclare(queue: _paymentQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var profileConsumer = new EventingBasicConsumer(_profileChannel);
            profileConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<ProfileCompletedMessageDTO>(content);
                await HandleProfileMessage(message);
                _profileChannel.BasicAck(ea.DeliveryTag, false);
            };

            var paymentConsumer = new EventingBasicConsumer(_paymentChannel);
            paymentConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<PaymentCompletedMessageDTO>(content);
                await HandlePaymentMessage(message);
                _paymentChannel.BasicAck(ea.DeliveryTag, false);
            };

            _profileChannel.BasicConsume(_profileQueueName, false, profileConsumer);
            _paymentChannel.BasicConsume(_paymentQueueName, false, paymentConsumer);

            return Task.CompletedTask;
        }

        private async Task HandleProfileMessage(ProfileCompletedMessageDTO message)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            Console.WriteLine($"Profile completed for user {message.UserId}");
            await userService.UpdateUserProfileStatus(message.UserId);
        }

        private async Task HandlePaymentMessage(PaymentCompletedMessageDTO message)
        {
            using var scope = _scopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            Console.WriteLine($"Payment completed for user {message.UserId}");
            await userService.UpdateUserPremiumStatus(message.UserId);
        }

        public override void Dispose()
        {
            _profileChannel.Close();
            _paymentChannel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
