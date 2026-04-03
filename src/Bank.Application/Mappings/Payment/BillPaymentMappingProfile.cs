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
/// AutoMapper profile for BillPayment entity mappings
/// </summary>
public class BillPaymentMappingProfile : Profile
{
    public BillPaymentMappingProfile()
    {
        // BillPayment to BillPaymentDto mapping
        CreateMap<BillPayment, BillPaymentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BillerId, opt => opt.MapFrom(src => src.BillerId))
            .ForMember(dest => dest.BillerName, opt => opt.MapFrom(src => src.Biller != null ? src.Biller.Name : string.Empty))
            .ForMember(dest => dest.BillerCategory, opt => opt.MapFrom(src => src.Biller != null ? src.Biller.Category : Bank.Domain.Enums.BillerCategory.Other))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ForMember(dest => dest.ProcessedDate, opt => opt.MapFrom(src => src.ProcessedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.RecurringPaymentId, opt => opt.MapFrom(src => src.RecurringPaymentId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();

        // BillPayment to BillPaymentHistoryDto mapping
        CreateMap<BillPayment, BillPaymentHistoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BillerName, opt => opt.MapFrom(src => src.Biller != null ? src.Biller.Name : string.Empty))
            .ForMember(dest => dest.BillerCategory, opt => opt.MapFrom(src => src.Biller != null ? src.Biller.Category : Bank.Domain.Enums.BillerCategory.Other))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ForMember(dest => dest.ProcessedDate, opt => opt.MapFrom(src => src.ProcessedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsRecurring, opt => opt.MapFrom(src => src.RecurringPaymentId.HasValue));

        CreateMap<BillPayment, CreateBillPaymentRequest>().ReverseMap();
        CreateMap<BillPayment, UpdateBillPaymentRequest>().ReverseMap();
    }
}

