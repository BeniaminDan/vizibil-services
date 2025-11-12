using AutoMapper;
using vizibil_api.Entities;
using vizibil_api.Models;

namespace vizibil_api.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<ContactModel, Contact>().ReverseMap();
    }
}