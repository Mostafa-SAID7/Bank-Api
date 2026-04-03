using AutoMapper;
using Bank.Application.DTOs.Account;
using Bank.Domain.Entities.Account;

namespace Bank.Application.Mappings.Account;

/// <summary>
/// AutoMapper profile for Account entity mappings
/// </summary>
public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Domain.Entities.Account, AccountDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
            .ForMember(dest => dest.AccountHolderName, opt => opt.MapFrom(src => src.AccountHolderName))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();

        CreateMap<Domain.Entities.Account, CreateAccountRequest>().ReverseMap();
        CreateMap<Domain.Entities.Account, UpdateAccountRequest>().ReverseMap();
    }
}
