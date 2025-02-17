using RabbitMQ.Client;
using Report.Application.DTOs;
using System.Text;
using System.Text.Json;

namespace PhoneDirectory.API.Services
{
    public class ReportRequestPublisher
    {
        private readonly IConfiguration _configuration;

        public ReportRequestPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendReportRequest(Guid personId, string location)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration["RabbitMQ:Uri"]) 
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            
            channel.QueueDeclare(queue: _configuration["RabbitMQ:RequestQueue"],
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = new
            {
                PersonId = personId,
                Location = location,
                RequestedDate = DateTime.UtcNow
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        
            channel.BasicPublish(exchange: "",
                                 routingKey: _configuration["RabbitMQ:RequestQueue"],
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" Rapor talepleri dinleniyor... {message}");


        }
    }
}
