using System.Text;
using System.Text.Json;
using vizibil_api.Models;
using RabbitMQ.Client;
using vizibil_api.Entities;
using vizibil_services.ApiService.Interfaces;

public class RabbitMqProducer(IConnection rabbitConnection) : IRabbitMqProducer
{
    public void PublishEmailTask(EmailMessage email)
    {
        const string queueName = "email-queue";
        
        var channel = rabbitConnection.CreateModel();

        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var messageBody = JsonSerializer.Serialize(email);
        var body = Encoding.UTF8.GetBytes(messageBody);

        channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, body: body);

        Console.WriteLine($" [x] Sent {messageBody}");
    }

    public EmailMessage CreateEmailBody(Contact contact)
    {
        var placeholders = new Dictionary<string, string>
        {
            { "Name", $"{contact.FirstName} {contact.LastName}" },
            { "Email", contact.Email },
            { "Phone", $"{contact.CountryCode} {contact.Phone}" },
            { "Website", contact.Website ?? "N/A" },
            { "Budget", contact.Budget ?? "N/A" },
            { "Country", contact.Country ?? "N/A" },
            { "Service", contact.Service },
            { "Message", contact.Message }
        };
        
        var path = Path.Combine(AppContext.BaseDirectory, "Templates", "EmailTemplate.html");
        // Load the HTML template
        var emailBody = File.ReadAllText(path);

        // Replace placeholders with actual values
        emailBody = placeholders.Aggregate(emailBody, (current, placeholder) => current.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value));
        
        return new EmailMessage
        {
            To = "beniamin.dan.one@gmail.com",
            Subject = "Contact Form Submission" + " - " + contact.Email,
            Body = emailBody
        };
    }
}