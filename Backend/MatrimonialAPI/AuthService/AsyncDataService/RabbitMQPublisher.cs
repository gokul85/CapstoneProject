using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using AuthService.Models.DTOs;

namespace AuthService.AsyncDataService
{
    public class RabbitMQPublisher
    {
        private IConnection _connection;
        private readonly string _authQueueName;
        private IModel _channel;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _authQueueName = configuration["RabbitMQ:AuthQueueName"];
            CreateConnection(configuration);
        }

        private void CreateConnection(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMQ:Host"] };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _authQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishRegisterUserProfileMessage(RegisterUserProfileDTO message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(exchange: "", routingKey: _authQueueName, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
