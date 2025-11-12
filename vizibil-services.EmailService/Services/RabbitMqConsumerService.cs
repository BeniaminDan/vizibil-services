using System.Text;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using vizibil_services.EmailService.Models;

namespace vizibil_services.EmailService.Services;

public class RabbitMqConsumerService(
    ILogger<RabbitMqConsumerService> logger,
    IConfiguration config,
    IServiceProvider serviceProvider,
    IConnection? messageConnection,
    IConfiguration configuration)
    : BackgroundService
{
    private readonly IConfiguration _config = config;
    private IConnection? _messageConnection = messageConnection;
    private IModel? _messageChannel;
    private EventingBasicConsumer? _consumer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string queueName = "email-queue";

        _messageConnection = serviceProvider.GetRequiredService<IConnection>();

        _messageChannel = _messageConnection.CreateModel();
        _messageChannel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _consumer = new EventingBasicConsumer(_messageChannel);
        _consumer.Received += ProcessMessageAsync;

        _messageChannel.BasicConsume(queue:  queueName,
            autoAck: true, 
            consumer: _consumer);
        
        logger.LogInformation(" [*] Waiting for messages...");

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (_consumer != null) _consumer.Received -= ProcessMessageAsync;
        _messageChannel?.Dispose();
        return Task.CompletedTask;
    }

    private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var email = JsonSerializer.Deserialize<EmailMessage>(message);

        logger.LogInformation($" [x] Received {message}");
        if (email != null) SendEmail(email);
    }

    private void SendEmail(EmailMessage email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(configuration["EmailService:FromName"], configuration["EmailService:FromEmail"]));
        message.To.Add(new MailboxAddress(configuration["EmailService:ToName"], configuration["EmailService:ToEmail"]));
        message.Subject = email.Subject;
        message.Body = new TextPart("html")
        {
            Text = email.Body
        };

        using var client = new SmtpClient();
        var port = configuration["EmailService:SmtpPort"] != null ? int.Parse(configuration["EmailService:SmtpPort"] ?? string.Empty) : 587;
        client.Connect(configuration["EmailService:SmtpServer"], port, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(configuration["EmailService:SmtpUsername"], configuration["EmailService:SmtpPassword"]);
        client.Send(message);
        client.Disconnect(true);
        
        logger.LogInformation($" [x] Email sent to {email.To}");
    }
}