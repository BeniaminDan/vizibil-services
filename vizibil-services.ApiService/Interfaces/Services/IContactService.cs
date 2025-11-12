
using vizibil_api.Entities;
using vizibil_api.Models;

namespace vizibil_api.Interfaces.Services;

public interface IContactService
{
    Task<Contact> Create(Contact trigger);
}