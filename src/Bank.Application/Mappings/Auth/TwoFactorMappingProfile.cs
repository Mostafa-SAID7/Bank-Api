using AutoMapper;
using Bank.Application.DTOs;
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.DTOs.Auth.TwoFactor;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Auth
{
    /// <summary>
    /// AutoMapper profile for TwoFactorToken entity mappings
    /// </summary>
    public class TwoFactorMappingProfile : Profile
    {
        public TwoFactorMappingProfile()
        {
            CreateMap<TwoFactorToken, TwoFactorTokenDto>().ReverseMap();
            CreateMap<TwoFactorToken, CreateTwoFactorTokenRequest>().ReverseMap();
            CreateMap<TwoFactorToken, TwoFactorTokenResult>().ReverseMap();
        }
    }
}
