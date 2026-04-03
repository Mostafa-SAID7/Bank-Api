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
        // Beneficiary to BeneficiaryDto mapping
        CreateMap<Beneficiary, BeneficiaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
            .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
            .ForMember(dest => dest.SwiftCode, opt => opt.MapFrom(src => src.SwiftCode))
            .ForMember(dest => dest.IbanNumber, opt => opt.MapFrom(src => src.IbanNumber))
            .ForMember(dest => dest.RoutingNumber, opt => opt.MapFrom(src => src.RoutingNumber))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.IsVerified))
            .ForMember(dest => dest.VerifiedDate, opt => opt.MapFrom(src => src.VerifiedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.DailyTransferLimit, opt => opt.MapFrom(src => src.DailyTransferLimit))
            .ForMember(dest => dest.MonthlyTransferLimit, opt => opt.MapFrom(src => src.MonthlyTransferLimit))
            .ForMember(dest => dest.SingleTransferLimit, opt => opt.MapFrom(src => src.SingleTransferLimit))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.LastTransferDate, opt => opt.MapFrom(src => src.LastTransferDate))
            .ForMember(dest => dest.TransferCount, opt => opt.MapFrom(src => src.TransferCount))
            .ForMember(dest => dest.TotalTransferAmount, opt => opt.MapFrom(src => src.TotalTransferAmount))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ReverseMap();

        CreateMap<Beneficiary, CreateBeneficiaryRequest>().ReverseMap();
        CreateMap<Beneficiary, UpdateBeneficiaryRequest>().ReverseMap();
    }
}

