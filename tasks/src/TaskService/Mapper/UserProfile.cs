using AutoMapper;
using Cashflow.Common.Events.Accounts;
using TaskService.Data.Models;
using TaskService.Data.Models.External;

namespace TaskService.Mapper
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
