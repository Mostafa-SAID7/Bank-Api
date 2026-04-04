using AutoMapper;
using Bank.Application.DTOs.Card.Core;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Card;

public class CardMappingProfile : Profile
{
    public CardMappingProfile()
    {
        CreateMap<Bank.Domain.Entities.Card, CardDto>().ReverseMap();
        CreateMap<Bank.Domain.Entities.Card, CreateCardRequest>().ReverseMap();
        CreateMap<Bank.Domain.Entities.Card, UpdateCardRequest>().ReverseMap();
    }
}
