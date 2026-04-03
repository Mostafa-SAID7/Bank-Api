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
        services.AddAutoMapper(config =>
        {
            // Account Mappings
            config.AddProfile<AccountMappingProfile>();
            config.AddProfile<ProfileMappingProfile>();
            config.AddProfile<JointAccountMappingProfile>();

            // Auth Mappings
            config.AddProfile<UserMappingProfile>();
            config.AddProfile<SessionMappingProfile>();
            config.AddProfile<TwoFactorMappingProfile>();

            // Card Mappings
            config.AddProfile<CardMappingProfile>();
            config.AddProfile<CardTransactionMappingProfile>();

            // Loan Mappings
            config.AddProfile<LoanMappingProfile>();
            config.AddProfile<LoanPaymentMappingProfile>();

            // Payment Mappings
            config.AddProfile<BeneficiaryMappingProfile>();
            config.AddProfile<BillPaymentMappingProfile>();
            config.AddProfile<RecurringPaymentMappingProfile>();

            // Transaction Mappings
            config.AddProfile<TransactionMappingProfile>();

            // Deposit Mappings
            config.AddProfile<DepositMappingProfile>();
            config.AddProfile<DepositTransactionMappingProfile>();

            // Statement Mappings
            config.AddProfile<StatementMappingProfile>();

            // Shared Mappings
            config.AddProfile<NotificationMappingProfile>();
            config.AddProfile<AuditLogMappingProfile>();
        });

        return services;
    }
}
