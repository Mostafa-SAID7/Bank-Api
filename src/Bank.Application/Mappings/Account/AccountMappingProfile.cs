using AutoMapper;
using Bank.Application.DTOs.Account.Core;
using AccountClass = Bank.Domain.Entities.Account;

namespace Bank.Application.Mappings.Account;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<AccountClass, AccountDto>().ReverseMap();
        CreateMap<AccountClass, CreateAccountRequest>().ReverseMap();
        CreateMap<AccountClass, UpdateAccountRequest>().ReverseMap();
    }
}
