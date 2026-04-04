using AutoMapper;
using Bank.Application.DTOs.Transaction.Core;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Transaction;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<Domain.Entities.Transaction, TransactionDto>().ReverseMap();
    }
}
