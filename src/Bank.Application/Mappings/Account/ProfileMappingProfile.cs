using AutoMapper;
using Bank.Application.DTOs.Account.Core;
using Bank.Application.DTOs.Account.Validation;
using Bank.Application.DTOs.Account.Lockout;
using Bank.Application.DTOs.Account.Profile;
using Bank.Application.DTOs.Account.JointAccount;
using Bank.Application.DTOs.Account.Transfer;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Account;

/// <summary>
/// AutoMapper profile for User profile mappings
/// </summary>
public class ProfileMappingProfile : Profile
{
    public ProfileMappingProfile()
    {
        CreateMap<User, ProfileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();

        CreateMap<User, UpdateProfileRequest>().ReverseMap();
    }
}

