using vizibil_api.Entities;
using vizibil_api.Models;

namespace vizibil_services.ApiService.Interfaces;

public interface IRabbitMqProducer
{
    void PublishEmailTask(EmailMessage email);
    
    EmailMessage CreateEmailBody(Contact contact);
}