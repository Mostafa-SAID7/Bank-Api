using Bank.Application.Mappings.Account;
using Bank.Application.Mappings.Auth;
using Bank.Application.Mappings.Card;
using Bank.Application.Mappings.Deposit;
using Bank.Application.Mappings.Loan;
using Bank.Application.Mappings.Payment;
using Bank.Application.Mappings.Shared;
using Bank.Application.Mappings.Statement;
using Bank.Application.Mappings.Transaction;

namespace Bank.Api.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for AutoMapper service registration
/// </summary>
public static class AutoMapperServiceExtensions
{
    /// <summary>
    /// Register AutoMapper with all domain-based mapping profiles
    /// </summary>
    public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(AccountMappingProfile).Assembly);

        return services;
    }
}
