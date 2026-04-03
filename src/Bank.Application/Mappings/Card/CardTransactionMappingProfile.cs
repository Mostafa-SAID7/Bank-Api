using AutoMapper;
using Bank.Application.DTOs.Card;
using Bank.Domain.Entities.Card;

namespace Bank.Application.Mappings.Card;

/// <summary>
/// AutoMapper profile for CardTransaction entity mappings
/// </summary>
public class CardTransactionMappingProfile : Profile
{
    public CardTransactionMappingProfile()
    {
        CreateMap<CardTransaction, CardTransactionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.CardId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();
    }
}
