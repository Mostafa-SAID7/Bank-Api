using AutoMapper;
using Bank.Application.DTOs.Account.Core;
using Bank.Application.DTOs.Account.Validation;
using Bank.Application.DTOs.Account.Lockout;
using Bank.Application.DTOs.Account.Profile;
using Bank.Application.DTOs.Account.JointAccount;
using Bank.Application.DTOs.Account.Transfer;
using Bank.Domain.Entities.Account;

namespace Bank.Application.Mappings.Account;

/// <summary>
/// AutoMapper profile for JointAccount entity mappings
/// </summary>
public class JointAccountMappingProfile : Profile
{
    public JointAccountMappingProfile()
    {
        CreateMap<JointAccount, JointAccountDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.PrimaryOwnerId, opt => opt.MapFrom(src => src.PrimaryOwnerId))
            .ForMember(dest => dest.SecondaryOwnerId, opt => opt.MapFrom(src => src.SecondaryOwnerId))
            .ReverseMap();

        CreateMap<JointAccount, CreateJointAccountRequest>().ReverseMap();
    }
}

