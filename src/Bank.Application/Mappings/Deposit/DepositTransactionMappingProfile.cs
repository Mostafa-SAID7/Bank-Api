using AutoMapper;
using Bank.Application.DTOs.Deposit;
using Bank.Domain.Entities.Deposit;

namespace Bank.Application.Mappings.Deposit;

/// <summary>
/// AutoMapper profile for DepositTransaction entity mappings
/// </summary>
public class DepositTransactionMappingProfile : Profile
{
    public DepositTransactionMappingProfile()
    {
        CreateMap<DepositTransaction, DepositTransactionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DepositId, opt => opt.MapFrom(src => src.DepositId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
            .ReverseMap();
    }
}
