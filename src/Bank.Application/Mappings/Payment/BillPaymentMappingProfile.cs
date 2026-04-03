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
        CreateMap<BillPayment, BillPaymentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BillerId, opt => opt.MapFrom(src => src.BillerId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
            .ReverseMap();

        CreateMap<BillPayment, CreateBillPaymentRequest>().ReverseMap();
        CreateMap<BillPayment, UpdateBillPaymentRequest>().ReverseMap();
    }
}

