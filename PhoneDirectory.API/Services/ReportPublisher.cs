using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace PhoneDirectory.API.Services
{
    public class ReportPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "report_exchange";
        private const string QueueName = "report_queue";

        public ReportPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(QueueName, false, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, "");
        }

        public void PublishReportRequest(Guid personId, string location)
        {
            var message = new
            {
                PersonId = personId,
                RequestedDate = DateTime.UtcNow,
                Location = location
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            _channel.BasicPublish(exchange: ExchangeName,
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine($"[X] Rapor talebi gönderildi: {personId} - {location}");
        }
    }
}
