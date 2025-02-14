using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PhoneDirectory.API.Services
{
    public class MessagePublisher:IMessagePublisher
    {
        private readonly IModel _channel;

        public MessagePublisher(RabbitMQClient rabbitMQClient)
        {
            _channel = rabbitMQClient.GetChannel() ?? throw new InvalidOperationException("Failed to get a valid channel from RabbitMQClient.");
            try
            {
                _channel.ExchangeDeclare(exchange: "directory_exchange", type: ExchangeType.Fanout);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while declaring the exchange: {ex.Message}");
                throw;
            }
        }

        public void Publish<T>(T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            try
            {
                _channel.BasicPublish(exchange: "directory_exchange", routingKey: "", basicProperties: null, body: body);
                Console.WriteLine($"Message published: {JsonSerializer.Serialize(message)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while publishing the message: {ex.Message}");
                throw;
            }
        }
    }
}
