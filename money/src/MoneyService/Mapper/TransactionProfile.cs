using AutoMapper;
using MoneyService.Data.Models;
using MoneyService.Dtos;

namespace MoneyService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<UserTransactionReadDto, UserTransaction>().ReverseMap();
            CreateMap<TaskTransactionReadDto, TaskTransaction>().ReverseMap();
        }
    }
}
