using AutoMapper;
using Bank.Application.DTOs.Payment;
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
