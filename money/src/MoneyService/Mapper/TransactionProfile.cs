using AutoMapper;
using MoneyService.Data.Models;
using MoneyService.Dtos;

namespace MoneyService.Mapper
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<UserTransactionReadDto, UserTransaction>().ReverseMap();
            CreateMap<TaskTransactionReadDto, TaskTransaction>().ReverseMap();
        }
    }
}
