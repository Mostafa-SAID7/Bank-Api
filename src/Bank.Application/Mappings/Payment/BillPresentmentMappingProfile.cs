using AutoMapper;
using Bank.Application.DTOs;
using Bank.Application.DTOs.Payment.BillPresentment;
using Bank.Domain.Entities;
using DomainBillPresentmentStatus = Bank.Domain.Enums.BillPresentmentStatus;
using DTOBillPresentmentStatus = Bank.Application.DTOs.BillPresentmentStatus;

namespace Bank.Application.Mappings.Payment;

/// <summary>
/// AutoMapper profile for BillPresentment entity mappings
/// </summary>
public class BillPresentmentMappingProfile : Profile
{
    public BillPresentmentMappingProfile()
    {
        // BillPresentment to BillPresentmentDto mapping
        CreateMap<BillPresentment, BillPresentmentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BillerId, opt => opt.MapFrom(src => src.BillerId))
            .ForMember(dest => dest.BillerName, opt => opt.MapFrom(src => src.Biller != null ? src.Biller.Name : string.Empty))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
            .ForMember(dest => dest.AmountDue, opt => opt.MapFrom(src => src.AmountDue))
            .ForMember(dest => dest.MinimumPayment, opt => opt.MapFrom(src => src.MinimumPayment))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.StatementDate, opt => opt.MapFrom(src => src.StatementDate))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ConvertBillPresentmentStatusToDto(src.Status)))
            .ForMember(dest => dest.BillNumber, opt => opt.MapFrom(src => src.BillNumber))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.PaidDate))
            .ReverseMap();
    }

    /// <summary>
    /// Converts a domain BillPresentmentStatus to a DTO BillPresentmentStatus
    /// </summary>
    private static DTOBillPresentmentStatus ConvertBillPresentmentStatusToDto(DomainBillPresentmentStatus domainStatus)
    {
        return domainStatus switch
        {
            DomainBillPresentmentStatus.Pending => DTOBillPresentmentStatus.Pending,
            DomainBillPresentmentStatus.Presented => DTOBillPresentmentStatus.Available,
            DomainBillPresentmentStatus.Viewed => DTOBillPresentmentStatus.Available,
            DomainBillPresentmentStatus.Paid => DTOBillPresentmentStatus.Paid,
            DomainBillPresentmentStatus.Overdue => DTOBillPresentmentStatus.Overdue,
            DomainBillPresentmentStatus.Cancelled => DTOBillPresentmentStatus.Cancelled,
            _ => DTOBillPresentmentStatus.Pending
        };
    }
}
