using AutoMapper;
using Cashflow.Common.Events.Money;
using TaskService.Data.Models.External;

namespace TaskService.Mapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TaskTransactionCreatedEvent, TaskTransaction>().ReverseMap();
            CreateMap<UserTransactionCreatedEvent, UserTransaction>().ReverseMap();
        }
    }
}
