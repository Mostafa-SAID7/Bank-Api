using AutoMapper;
using Bank.Application.DTOs.Deposit.Core;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Deposit;

public class DepositMappingProfile : Profile
{
    public DepositMappingProfile()
    {
        CreateMap<Bank.Domain.Entities.FixedDeposit, DepositDto>().ReverseMap();
    }
}
