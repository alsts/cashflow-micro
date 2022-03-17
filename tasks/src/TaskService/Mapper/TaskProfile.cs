using AutoMapper;
using TaskService.Data.Models;
using TaskService.Dtos.Income;
using TaskService.Dtos.Promotion;

namespace TaskService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<IncomeTaskDto, Task>().ReverseMap();
            CreateMap<Task, PromotionTaskDto>().ForMember(o => o.AuthorId, b => b.MapFrom(z => z.PublicId));
        }
    }
}
