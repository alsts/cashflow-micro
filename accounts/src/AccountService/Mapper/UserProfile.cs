using AccountService.Data.Models;
using AccountService.Dtos;
using AutoMapper;
using Cashflow.Common.Events.Accounts;

namespace AccountService.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserCreatedEvent>().ReverseMap();
            CreateMap<User, UserUpdatedEvent>().ReverseMap();
            CreateMap<User, UserUpdatedEvent>().ReverseMap();
            
            CreateMap<User, UserReadDto>().ForMember(o => o.Id, b => b.MapFrom(z => z.PublicId));
        }
    }
}
