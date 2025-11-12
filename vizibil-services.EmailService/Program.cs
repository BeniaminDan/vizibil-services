using vizibil_services.EmailService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient(connectionName: "messaging");

builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

app.Run();