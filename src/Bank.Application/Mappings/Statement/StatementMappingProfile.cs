using AutoMapper;
using Bank.Application.DTOs.Statement;
using Bank.Domain.Entities.Statement;

namespace Bank.Application.Mappings.Statement;

/// <summary>
/// AutoMapper profile for Statement entity mappings
/// </summary>
public class StatementMappingProfile : Profile
{
    public StatementMappingProfile()
    {
        CreateMap<Statement, StatementDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.OpeningBalance, opt => opt.MapFrom(src => src.OpeningBalance))
            .ForMember(dest => dest.ClosingBalance, opt => opt.MapFrom(src => src.ClosingBalance))
            .ReverseMap();

        CreateMap<Statement, CreateStatementRequest>().ReverseMap();
    }
}
