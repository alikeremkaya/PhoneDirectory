using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Report.Application.Services
{
    public class ReportResultPublisher
    {
        private readonly IConfiguration _configuration;

        public ReportResultPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendReportResult(Guid reportId, string status)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration["RabbitMQ:Uri"])
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _configuration["RabbitMQ:ResultQueue"],
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = new
            {
                ReportId = reportId,
                Status = status,
                CompletedDate = DateTime.UtcNow
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "",
                                 routingKey: _configuration["RabbitMQ:ResultQueue"],
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" Rapor sonucu gönderildi: {JsonSerializer.Serialize(message)}");
        }
    }
}