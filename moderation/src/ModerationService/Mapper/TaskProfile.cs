using AutoMapper;
using Cashflow.Common.Events.Moderation;
using Task = System.Threading.Tasks.Task;

namespace ModerationService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskApprovedEvent, Task>().ReverseMap();
        }
    }
}
