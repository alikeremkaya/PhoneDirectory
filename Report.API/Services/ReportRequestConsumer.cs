using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Report.Application.DTOs;
using Report.Application.Services;
using System.Text;
using System.Text.Json;

public class ReportRequestConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public ReportRequestConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    public void StartListening()
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(_configuration["RabbitMQ:Uri"])
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _configuration["RabbitMQ:RequestQueue"],
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine("🔹 Yeni mesaj alındı!");
            Console.WriteLine($" Gelen Mesaj: {message}");

            try
            {
              
                var reportRequest = JsonSerializer.Deserialize<ReportRequest>(message);

                Console.WriteLine($" Person ID: {reportRequest.PersonId}");
                Console.WriteLine($" Konum: {reportRequest.Location}");
                Console.WriteLine($"Talep Zamanı: {reportRequest.RequestedDate}");

               
                using (var scope = _scopeFactory.CreateScope())
                {
                    var reportService = scope.ServiceProvider.GetRequiredService<IReportApplicationService>();
                    await ProcessReport(reportRequest, reportService);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Mesaj işlenirken hata oluştu: {ex.Message}");
            }
        };

        channel.BasicConsume(queue: _configuration["RabbitMQ:RequestQueue"],
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine($" Rapor talepleri dinleniyor... ");
    }

    public async Task ProcessReport(ReportRequest reportRequest, IReportApplicationService reportService)
    {
        Console.WriteLine(" Rapor oluşturma işlemi başladı...");

        var createReportDto = new CreateReportDTO
        {
            Location = reportRequest.Location
        };

        var result = await reportService.CreateReportAsync(createReportDto);

        if (result.IsSuccess)
        {
            Console.WriteLine($" Rapor başarıyla oluşturuldu: {result.Data.Id}");
        }
        else
        {
            Console.WriteLine($" Rapor oluşturulamadı: {result.Messages}");
        }
    }
}
