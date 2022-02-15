using AutoMapper;
using Cashflow.Common.Events.Accounts;
using TaskService.Data.Models;

namespace ModerationService.Mapper
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
