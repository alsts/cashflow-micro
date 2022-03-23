using AutoMapper;
using Cashflow.Common.Events.Accounts;
using Cashflow.Common.Events.Moderation;
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
            
            CreateMap<User, UserBanDto>().ForMember(o => o.UserId, b => b.MapFrom(z => z.PublicId));
            CreateMap<User, UserBannedEvent>()
                .ForMember(o => o.UserId, b => b.MapFrom(z => z.PublicId))
                .ForMember(o => o.BannedAt, b => b.MapFrom(z => z.BannedAt));
        }
    }
}
