using AutoMapper;
using Cashflow.Common.Events.Tasks;
using MoneyService.Data.Models.External;

namespace MoneyService.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskCreatedEvent, Task>().ReverseMap();
            CreateMap<TaskUpdatedEvent, Task>().ReverseMap();
        }
    }
}
