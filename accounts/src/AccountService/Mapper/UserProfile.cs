using AccountService.Data.Models;
using AutoMapper;
using Cashflow.Common.Events;

namespace AccountService.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreatedEvent, User>().ReverseMap();
            CreateMap<UserUpdatedEvent, User>().ReverseMap();
        }
    }
}
