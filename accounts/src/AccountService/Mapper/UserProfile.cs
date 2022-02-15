using AccountService.Data.Models;
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
        }
    }
}
