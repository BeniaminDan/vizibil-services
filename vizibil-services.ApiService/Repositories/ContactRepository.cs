using vizibil_api.Interfaces.Repositories;
using vizibil_api.Entities;
using vizibil_api.Models;

namespace vizibil_api.Repositories;

public class ContactRepository(DatabaseContext context) : IContactRepository
{

    public async Task<Contact> Create(Contact @event)
    {
        var entity = new Contact
        {
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            Email = @event.Email,
            CountryCode = @event.CountryCode,
            Phone = @event.Phone,
            Website = @event.Website,
            Budget = @event.Budget,
            Country = @event.Country,
            Service = @event.Service,
            Message = @event.Message,
        };
        
        await context.Contacts.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return new Contact
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            CountryCode = entity.CountryCode,
            Phone = entity.Phone,
            Website = entity.Website,
            Budget = entity.Budget,
            Country = entity.Country,
            Service = entity.Service,
            Message = entity.Message,
            Date = entity.Date
        };
    }
}