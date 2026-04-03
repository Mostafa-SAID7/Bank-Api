using AutoMapper;
using Bank.Application.DTOs.Payment;
using Bank.Domain.Entities.Payment;

namespace Bank.Application.Mappings.Payment;

/// <summary>
/// AutoMapper profile for RecurringPayment entity mappings
/// </summary>
public class RecurringPaymentMappingProfile : Profile
{
    public RecurringPaymentMappingProfile()
    {
        CreateMap<RecurringPayment, RecurringPaymentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BeneficiaryId, opt => opt.MapFrom(src => src.BeneficiaryId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();

        CreateMap<RecurringPayment, CreateRecurringPaymentRequest>().ReverseMap();
        CreateMap<RecurringPayment, UpdateRecurringPaymentRequest>().ReverseMap();
    }
}
