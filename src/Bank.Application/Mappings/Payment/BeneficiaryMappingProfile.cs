using AutoMapper;
using Bank.Application.DTOs.Payment.Core;
using Bank.Application.DTOs.Payment.Beneficiary;
using Bank.Application.DTOs.Payment.Biller;
using Bank.Application.DTOs.Payment.Batch;
using Bank.Application.DTOs.Payment.Routing;
using Bank.Application.DTOs.Payment.Receipt;
using Bank.Application.DTOs.Payment.Recurring;
using Bank.Application.DTOs.Payment.Template;
using Bank.Domain.Entities.Payment;

namespace Bank.Application.Mappings.Payment;

/// <summary>
/// AutoMapper profile for Beneficiary entity mappings
/// </summary>
public class BeneficiaryMappingProfile : Profile
{
    public BeneficiaryMappingProfile()
    {
        CreateMap<Beneficiary, BeneficiaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BeneficiaryName, opt => opt.MapFrom(src => src.BeneficiaryName))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
            .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
            .ReverseMap();

        CreateMap<Beneficiary, CreateBeneficiaryRequest>().ReverseMap();
        CreateMap<Beneficiary, UpdateBeneficiaryRequest>().ReverseMap();
    }
}

