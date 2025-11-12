using vizibil_api.Interfaces.Repositories;
using vizibil_api.Interfaces.Services;
using vizibil_api.Entities;
using vizibil_api.Models;

namespace vizibil_api.Services.Database;

public class ContactService(IContactRepository contactRepository) : IContactService
{
    public async Task<Contact> Create(Contact @event)
    {
        return await contactRepository.Create(@event);
    }
}