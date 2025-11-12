using vizibil_api.Entities;

namespace vizibil_api.Interfaces.Repositories;

public interface IContactRepository
{
    Task<Contact> Create(Contact document);
}