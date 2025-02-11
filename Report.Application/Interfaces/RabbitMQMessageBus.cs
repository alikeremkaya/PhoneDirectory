using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Report.Application.Interfaces
{
    public class RabbitMQMessageBus : IMessageBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMQConfiguration _config;
        private readonly Dictionary<string, EventingBasicConsumer> _consumers;

        public RabbitMQMessageBus(IOptions<RabbitMQConfiguration> options)
        {
            _config = options.Value;
            _consumers = new Dictionary<string, EventingBasicConsumer>();

            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.Username,
                Password = _config.Password,
                Port = 5672
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                throw new Exception("RabbitMQ bağlantısı kurulamadı: Broker unreachable", ex);
            }
            catch (RabbitMQ.Client.Exceptions.AuthenticationFailureException ex)
            {
                throw new Exception("RabbitMQ bağlantısı kurulamadı: Authentication failed", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("RabbitMQ bağlantısı kurulamadı", ex);
            }
        }

        public void PublishMessage<T>(T message, string queueName)
        {
            try
            {
                _channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);
            }
            catch (Exception ex)
            {
                throw new Exception($"Mesaj yayınlanamadı: {ex.Message}", ex);
            }
        }

        public void Subscribe<T>(string queueName, Action<T> onMessage)
        {
            try
            {
                _channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var deserializedMessage = JsonSerializer.Deserialize<T>(message);

                    try
                    {
                        onMessage(deserializedMessage);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception)
                    {
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };

                _channel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);

                _consumers[queueName] = consumer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Kuyruk dinlenirken hata oluştu: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_channel?.IsOpen ?? false)
                {
                    _channel.Close();
                    _channel.Dispose();
                }

                if (_connection?.IsOpen ?? false)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("RabbitMQ bağlantısı kapatılırken hata oluştu", ex);
            }
        }
    }
}
    
