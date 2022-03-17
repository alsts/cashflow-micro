using AutoMapper;
using TaskService.Data.Models;
using TaskService.Dtos.Income;

namespace TaskService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<IncomeTaskDto, Task>().ReverseMap();
            CreateMap<IncomeTaskJobDto, TaskJob>().ReverseMap();
        }
    }
}
