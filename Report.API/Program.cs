
using MassTransit;
using Report.API.Consumers;
using Report.Application.Extentions;
using Report.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
// MassTransit ve RabbitMQ konfigürasyonu
builder.Services.AddMassTransit(x =>
{
    // Consumer'ý kaydet
    x.AddConsumer<ReportRequestConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("RabbitMQ:Host"), "/", h =>
        {
            h.Username(builder.Configuration.GetValue<string>("RabbitMQ:guest"));
            h.Password(builder.Configuration.GetValue<string>("RabbitMQ:guest"));
        });

        // Consumer endpoint konfigürasyonu
        cfg.ConfigureEndpoints(context);
    });
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