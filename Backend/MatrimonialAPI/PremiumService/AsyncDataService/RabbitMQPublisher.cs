using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using PremiumService.Models.DTOs;
using PremiumService.Interfaces;

namespace PremiumService.AsyncDataService
{
    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly string _paymentQueueName;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:Host"] };
            _connection = factory.CreateConnection();
            _paymentQueueName = configuration["RabbitMQ:PaymentQueueName"];
        }

        public void PublishPaymentMessage(PaymentCompleteMessageDTO message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _paymentQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(exchange: "", routingKey: _paymentQueueName, basicProperties: null, body: body);
            }
        }
    }
}
