using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Report.API.Services
{
    public class MessageListener
    {
        private readonly IModel _channel;

        public MessageListener(RabbitMQClient rabbitMQClient)
        {
            _channel = rabbitMQClient.GetChannel();
            _channel.ExchangeDeclare(exchange: "exchange_name", type: ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "exchange_name", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received message: {0}", message);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Start()
        {
            // Start listening to messages
        }
    }
}
