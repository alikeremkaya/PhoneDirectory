using RabbitMQ.Client;
using Report.API;
using Report.API.Services;
using Report.Application.Extentions;

using Report.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSingleton<ReportConsumer>();
builder.Services.AddHostedService<ReportConsumer>();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "rabbitmq",  // Eðer localhost olarak ayarlýysa, "rabbitmq" olarak deðiþtir
        UserName = "guest",
        Password = "guest"
    };

    return factory.CreateConnection();
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();