using AutoMapper;
using Bank.Application.DTOs.Deposit.Core;
using Bank.Application.DTOs.Deposit.FixedDeposit;
using Bank.Application.DTOs.Deposit.Interest;
using Bank.Application.DTOs.Deposit.Maturity;
using Bank.Application.DTOs.Deposit.Withdrawal;
using Bank.Domain.Entities.Deposit;

namespace Bank.Application.Mappings.Deposit;

/// <summary>
/// AutoMapper profile for Deposit entity mappings
/// </summary>
public class DepositMappingProfile : Profile
{
    public DepositMappingProfile()
    {
        CreateMap<FixedDeposit, DepositDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
            .ForMember(dest => dest.MaturityDate, opt => opt.MapFrom(src => src.MaturityDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();

        CreateMap<FixedDeposit, CreateDepositRequest>().ReverseMap();
    }
}

