using AutoMapper;
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.DTOs.Auth.TwoFactor;
using Bank.Application.DTOs.Auth.Security;
using Bank.Application.DTOs.Auth.Session;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Auth;

/// <summary>
/// AutoMapper profile for User entity mappings
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();

        CreateMap<User, CreateUserRequest>().ReverseMap();
        CreateMap<User, UpdateUserRequest>().ReverseMap();
    }
}

