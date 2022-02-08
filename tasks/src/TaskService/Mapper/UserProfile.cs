using AutoMapper;
using Cashflow.Common.Events;
using TaskService.Data.Models;

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
