using AutoMapper;
using Cashflow.Common.Events.Accounts;
using MoneyService.Data.Models;

namespace MoneyService.Mapper
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
