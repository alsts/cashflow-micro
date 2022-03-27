using AutoMapper;
using Cashflow.Common.Events.Tasks;
using TaskService.Data.Models;
using TaskService.Dtos.Income;
using TaskService.Dtos.Promotion;

namespace TaskService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, IncomeTaskDto>()
                .ForMember(o => o.Id, b => b.MapFrom(z => z.PublicId))
                .ForMember(o => o.AuthorId, b => b.MapFrom(z => z.CreatedByUserId));
            
            CreateMap<Task, PromotionTaskDto>()
                .ForMember(o => o.Id, b => b.MapFrom(z => z.PublicId))
                .ForMember(o => o.AuthorId, b => b.MapFrom(z => z.CreatedByUserId));
            
            CreateMap<TaskCreatedEvent, Task>().ReverseMap();
            CreateMap<TaskUpdatedEvent, Task>().ReverseMap();
        }
    }
}
