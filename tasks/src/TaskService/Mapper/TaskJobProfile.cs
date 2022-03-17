using AutoMapper;
using TaskService.Data.Models;
using TaskService.Dtos.Income;
using TaskService.Dtos.Promotion;

namespace TaskService.Mapper
{
    public class TaskJobProfile : Profile
    {
        public TaskJobProfile()
        {
            CreateMap<IncomeTaskJobDto, TaskJob>().ReverseMap();
            CreateMap<PromotionTaskJobDto, TaskJob>().ReverseMap();
        }
    }
}
