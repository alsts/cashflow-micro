using AutoMapper;
using Cashflow.Common.Events.Accounts;
using Cashflow.Common.Events.Moderation;
using ModerationService.Data.Models;
using ModerationService.Data.Models.External;
using ModerationService.Dtos;

namespace ModerationService.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreatedEvent, User>().ReverseMap();
            CreateMap<UserUpdatedEvent, User>().ReverseMap();
            
            CreateMap<UserBanDto, UserBan>().ReverseMap();
            CreateMap<UserBannedEvent, UserBan>().ReverseMap();
        }
    }
}
