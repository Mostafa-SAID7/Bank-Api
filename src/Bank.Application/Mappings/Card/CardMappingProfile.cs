using AutoMapper;
using Bank.Application.DTOs.Card;
using Bank.Domain.Entities.Card;

namespace Bank.Application.Mappings.Card;

/// <summary>
/// AutoMapper profile for Card entity mappings
/// </summary>
public class CardMappingProfile : Profile
{
    public CardMappingProfile()
    {
        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
            .ForMember(dest => dest.CardHolderName, opt => opt.MapFrom(src => src.CardHolderName))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ReverseMap();

        CreateMap<Card, CreateCardRequest>().ReverseMap();
        CreateMap<Card, UpdateCardRequest>().ReverseMap();
    }
}
