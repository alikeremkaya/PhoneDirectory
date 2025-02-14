using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace PhoneDirectory.API.Services
{
    public class RabbitMQClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (BrokerUnreachableException ex)
            {
                // Log the exception and handle it appropriately
                Console.WriteLine($"Could not reach RabbitMQ broker: {ex.Message}");
                throw new InvalidOperationException("Failed to create a connection to RabbitMQ broker.", ex);
            }
            catch (Exception ex)
            {
                // Log the exception and handle it appropriately
                Console.WriteLine($"An error occurred while setting up RabbitMQ: {ex.Message}");
                throw new InvalidOperationException("Failed to set up RabbitMQ.", ex);
            }
        }

        public IModel GetChannel()
        {
            if (_channel == null || !_channel.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ channel is not open.");
            }
            return _channel;
        }
    }
}

