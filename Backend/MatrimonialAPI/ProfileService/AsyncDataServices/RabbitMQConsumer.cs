using ProfileService.Interfaces;
using ProfileService.Models.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace ProfileService.AsyncDataServices
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly string _authQueueName;
        private IConnection _connection;
        private IModel _authChannel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _authQueueName = configuration["RabbitMQ:AuthQueueName"];
            CreateConnection(configuration);
            _scopeFactory = scopeFactory;
        }

        private void CreateConnection(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:Host"], UserName = configuration["RabbitMQ:UserName"], Password = configuration["RabbitMQ:Password"] };
            _connection = factory.CreateConnection();
            _authChannel = _connection.CreateModel();
            _authChannel.QueueDeclare(queue: _authQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var authConsumer = new EventingBasicConsumer(_authChannel);
            authConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<RegisterUserProfileDTO>(content);
                await HandleAuthMessage(message);
                _authChannel.BasicAck(ea.DeliveryTag, false);
            };

            _authChannel.BasicConsume(_authQueueName, false, authConsumer);

            return Task.CompletedTask;
        }

        private async Task HandleAuthMessage(RegisterUserProfileDTO message)
        {
            using var scope = _scopeFactory.CreateScope();
            var userprofileservice = scope.ServiceProvider.GetRequiredService<IProfileService>();
            Console.WriteLine($"Profile Creation for {message.UserId}");
            await userprofileservice.CreateUserProfile(message);
        }

        public override void Dispose()
        {
            _authChannel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
