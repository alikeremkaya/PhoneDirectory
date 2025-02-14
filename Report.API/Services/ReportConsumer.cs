using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Report.Infrastructure.AppContext;

namespace Report.API.Services
{
    public class ReportConsumer:BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private const string ExchangeName = "report_exchange";
        private const string QueueName = "report_queue";

        public ReportConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(QueueName, false, false, false, null);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
                throw;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var reportRequest = JsonConvert.DeserializeObject<ReportRequestDTO>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();

                    var report = new  Domain.Entities.Report
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        ReportStatus =  Domain.Enums.ReportStatus.Completed,
                        PersonCount = 100, 
                        PhoneNumberCount = 200, 
                        Location = reportRequest.Location
                    };

                    dbContext.Reports.Add(report);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine($"[✔] Rapor tamamlandı: {report.Id} - {report.Location}");
                }
            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }

    public class ReportRequestDTO
    {
        public Guid PersonId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Location { get; set; }
    }
}
