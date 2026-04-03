using AutoMapper;
using Bank.Application.DTOs.Transaction.Core;
using Bank.Application.DTOs.Transaction.Search;
using Bank.Application.DTOs.Transaction.Analytics;
using Bank.Application.DTOs.Transaction.Fraud;
using Bank.Domain.Entities.Transaction;

namespace Bank.Application.Mappings.Transaction;

/// <summary>
/// AutoMapper profile for Transaction entity mappings
/// </summary>
public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FromAccountId, opt => opt.MapFrom(src => src.FromAccountId))
            .ForMember(dest => dest.ToAccountId, opt => opt.MapFrom(src => src.ToAccountId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();

        CreateMap<Transaction, CreateTransactionRequest>().ReverseMap();
    }
}

