using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using ProfileService.Models.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace ProfileService.AsyncDataServices
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly string _profileQueueName;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:Host"], UserName = configuration["RabbitMQ:UserName"], Password = configuration["RabbitMQ:Password"] };
            _connection = factory.CreateConnection();
            _profileQueueName = configuration["RabbitMQ:ProfileQueueName"];
        }

        public void PublishProfileMessage(ProfileCompletedMessageDTO message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _profileQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(exchange: "", routingKey: _profileQueueName, basicProperties: null, body: body);
            }
        }
    }
}
