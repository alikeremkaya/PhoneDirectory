using RabbitMQ.Client;
using Report.API;

using Report.Application.Extentions;
using Report.Application.Services;
using Report.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ReportRequestConsumer>();
builder.Services.AddSingleton<ReportResultPublisher>();


builder.Services.AddScoped<IReportApplicationService, ReportApplicationService>();

var app = builder.Build();


var consumer = new ReportRequestConsumer(app.Services.GetRequiredService<IConfiguration>(),
                                         app.Services.GetRequiredService<IServiceScopeFactory>());
consumer.StartListening();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
