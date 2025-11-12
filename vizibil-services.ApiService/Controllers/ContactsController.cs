using AutoMapper;
using vizibil_api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using vizibil_api.Entities;
using vizibil_api.Models;
using vizibil_services.ApiService.Interfaces;

namespace vizibil_api.Controllers;

[ApiController]
[Route("/contact")]
public class ContactsController(IContactService contactService, IMapper mapper, IRabbitMqProducer producer) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ContactModel eventModel)
    {
        var eventEntity = mapper.Map<Contact>(eventModel);
        var newEvent = await contactService.Create(eventEntity);
        
        var email = producer.CreateEmailBody(newEvent);
        
        producer.PublishEmailTask(email);

        return Ok("Contact sent successfully");
    }
}