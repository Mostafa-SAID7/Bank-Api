using AutoMapper;
using Bank.Application.DTOs.Loan.Core;
using Bank.Application.DTOs.Loan.Application;
using Bank.Application.DTOs.Loan.Approval;
using Bank.Application.DTOs.Loan.Disbursement;
using Bank.Application.DTOs.Loan.Repayment;
using Bank.Application.DTOs.Loan.Analytics;
using Bank.Application.DTOs.Loan.Configuration;
using Bank.Domain.Entities.Loan;

namespace Bank.Application.Mappings.Loan;

/// <summary>
/// AutoMapper profile for LoanPayment entity mappings
/// </summary>
public class LoanPaymentMappingProfile : Profile
{
    public LoanPaymentMappingProfile()
    {
        CreateMap<LoanPayment, LoanPaymentDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LoanId, opt => opt.MapFrom(src => src.LoanId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();

        CreateMap<LoanPayment, CreateLoanPaymentRequest>().ReverseMap();
    }
}

