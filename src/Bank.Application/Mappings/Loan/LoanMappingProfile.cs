using AutoMapper;
using Bank.Application.DTOs.Loan;
using Bank.Domain.Entities.Loan;

namespace Bank.Application.Mappings.Loan;

/// <summary>
/// AutoMapper profile for Loan entity mappings
/// </summary>
public class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        CreateMap<Loan, LoanDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LoanNumber, opt => opt.MapFrom(src => src.LoanNumber))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ReverseMap();

        CreateMap<Loan, CreateLoanRequest>().ReverseMap();
        CreateMap<Loan, UpdateLoanRequest>().ReverseMap();
    }
}
