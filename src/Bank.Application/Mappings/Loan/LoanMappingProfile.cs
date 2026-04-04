using AutoMapper;
using Bank.Application.DTOs.Loan.Core;
using Bank.Domain.Entities;

namespace Bank.Application.Mappings.Loan;

public class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        CreateMap<Domain.Entities.Loan, LoanDto>().ReverseMap();
    }
}
