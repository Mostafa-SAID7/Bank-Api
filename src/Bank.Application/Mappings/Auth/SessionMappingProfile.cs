using AutoMapper;
using Bank.Application.DTOs.Auth;
using Bank.Domain.Entities.Auth;

namespace Bank.Application.Mappings.Auth;

/// <summary>
/// AutoMapper profile for Session entity mappings
/// </summary>
public class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.ExpiresAt))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();
    }
}
