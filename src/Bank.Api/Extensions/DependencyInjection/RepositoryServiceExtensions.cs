using Bank.Domain.Interfaces;
using Bank.Domain.Interfaces.Account;
using Bank.Domain.Interfaces.Auth;
using Bank.Domain.Interfaces.Card;
using Bank.Domain.Interfaces.Loan;
using Bank.Domain.Interfaces.Payment;
using Bank.Domain.Interfaces.Shared;
using Bank.Domain.Interfaces.Statement;
using Bank.Infrastructure.Repositories;
using Bank.Infrastructure.Repositories.Account;
using Bank.Infrastructure.Repositories.Auth;
using Bank.Infrastructure.Repositories.Card;
using Bank.Infrastructure.Repositories.Deposit;
using Bank.Infrastructure.Repositories.Loan;
using Bank.Infrastructure.Repositories.Payment;
using Bank.Infrastructure.Repositories.Shared;
using Bank.Infrastructure.Repositories.Statement;

namespace Bank.Api.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for repository service registration
/// </summary>
public static class RepositoryServiceExtensions
{
    /// <summary>
    /// Register all repository services
    /// </summary>
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        // Unit of Work pattern
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Domain repositories
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IAccountLockoutRepository, AccountLockoutRepository>();
        services.AddScoped<IIpWhitelistRepository, IpWhitelistRepository>();
        services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();
        services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecurringPaymentRepository, RecurringPaymentRepository>();
        services.AddScoped<IPaymentTemplateRepository, PaymentTemplateRepository>();
        services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
        services.AddScoped<IStatementRepository, StatementRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICardTransactionRepository, CardTransactionRepository>();
        services.AddScoped<IBillerRepository, BillerRepository>();
        services.AddScoped<IBillPaymentRepository, BillPaymentRepository>();
        services.AddScoped<IPaymentRetryRepository, PaymentRetryRepository>();
        services.AddScoped<IPaymentReceiptRepository, PaymentReceiptRepository>();
        services.AddScoped<IBillerHealthCheckRepository, BillerHealthCheckRepository>();
        services.AddScoped<IBillPresentmentRepository, BillPresentmentRepository>();
        services.AddScoped<IDepositProductRepository, DepositProductRepository>();
        services.AddScoped<IFixedDepositRepository, FixedDepositRepository>();
        services.AddScoped<IInterestTierRepository, InterestTierRepository>();
        services.AddScoped<IDepositTransactionRepository, DepositTransactionRepository>();
        services.AddScoped<IDepositCertificateRepository, DepositCertificateRepository>();
        services.AddScoped<IMaturityNoticeRepository, MaturityNoticeRepository>();

        return services;
    }
}
