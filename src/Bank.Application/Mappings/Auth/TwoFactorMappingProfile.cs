using AutoMapper;
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.DTOs.Auth.TwoFactor;
using Bank.Application.DTOs.Auth.Security;
using Bank.Application.DTOs.Auth.Session;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Auth;

/// <summary>
/// AutoMapper profile for TwoFactorToken entity mappings
/// </summary>
public class TwoFactorMappingProfile : Profile
{
    public TwoFactorMappingProfile()
    {
        CreateMap<TwoFactorToken, TwoFactorTokenDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt))
            .ReverseMap();

        CreateMap<TwoFactorToken, CreateTwoFactorTokenRequest>().ReverseMap();
    }
}


