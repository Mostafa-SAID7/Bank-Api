using AutoMapper;
using Bank.Application.DTOs.Payment.Receipt;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Payment;

/// <summary>
/// AutoMapper profile for PaymentReceipt entity mappings
/// </summary>
public class PaymentReceiptMappingProfile : Profile
{
    public PaymentReceiptMappingProfile()
    {
        // PaymentReceipt to PaymentReceiptDto mapping
        CreateMap<PaymentReceipt, PaymentReceiptDto>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(dest => dest.ReceiptNumber, opt => opt.MapFrom(src => src.ReceiptNumber))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.BillerName, opt => opt.MapFrom(src => src.BillerName))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.ProcessedDate, opt => opt.MapFrom(src => src.ProcessedDate))
            .ForMember(dest => dest.ConfirmationNumber, opt => opt.MapFrom(src => src.ConfirmationNumber))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.ProcessingFee, opt => opt.MapFrom(src => src.ProcessingFee))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();
    }
}

