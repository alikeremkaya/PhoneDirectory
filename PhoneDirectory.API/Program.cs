using PhoneDirectory.Infrastructure.Extentions;
using PhoneDirectory.Application.Extentions;
using PhoneDirectory.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(sp =>
{
    var config = builder.Configuration;
    return new RabbitMQClient(
        config["RabbitMQ:HostName"],
        config["RabbitMQ:Username"],
        config["RabbitMQ:Password"]
    );
});

builder.Services.AddSingleton<MessagePublisher>();
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