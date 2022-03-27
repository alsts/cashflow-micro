using AutoMapper;
using Cashflow.Common.Events.Moderation;
using Cashflow.Common.Events.Tasks;
using Task = ModerationService.Data.Models.External.Task;

namespace ModerationService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreatedEvent, Task>().ReverseMap();
            CreateMap<TaskUpdatedEvent, Task>().ReverseMap();
            
            CreateMap<Task, TaskApprovedEvent>()
                .ForMember(o => o.TaskId, b => b.MapFrom(z => z.PublicId))
                .ForMember(o => o.ApprovedAt, b => b.MapFrom(z => z.LastUpdatedAt));
        }
    }
}
