using AutoMapper;
using Cashflow.Common.Events.Accounts;
using Cashflow.Common.Events.Moderation;
using ModerationService.Data.Models;

namespace ModerationService.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserBlockedEvent, User>().ReverseMap();
        }
    }
}
