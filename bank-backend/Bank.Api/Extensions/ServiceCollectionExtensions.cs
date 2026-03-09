using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Hangfire;
using Bank.Domain.Entities;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Bank.Infrastructure.Repositories;
using Bank.Application.Interfaces;

namespace Bank.Api.Extensions;

/// <summary>
/// Extension methods for organizing service registrations
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register database and Entity Framework services
    /// </summary>
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    /// <summary>
    /// Register caching and session services
    /// </summary>
    public static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Redis for caching and rate limiting
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        });

        // Session support
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }

    /// <summary>
    /// Register authentication and authorization services
    /// </summary>
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ASP.NET Core Identity
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<BankDbContext>()
        .AddDefaultTokenProviders();

        // JWT Authentication
        services.AddAuthentication(opts => {
            opts.DefaultAuthenticateScheme = "Bearer";
            opts.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(options => {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "SUPER_SECRET_KEY");
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"]
            };
        });

        return services;
    }

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

    /// <summary>
    /// Register all application services (business logic)
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Authentication & Authorization Services
        services.AddScoped<Bank.Application.Interfaces.IAuthService, Bank.Application.Services.AuthService>();
        services.AddScoped<Bank.Application.Interfaces.ITwoFactorAuthService, Bank.Application.Services.TwoFactorAuthService>();
        services.AddScoped<Bank.Application.Interfaces.ISessionService, Bank.Application.Services.SessionService>();

        // Account Management Services
        services.AddScoped<Bank.Application.Interfaces.IAccountService, Bank.Application.Services.AccountService>();
        services.AddScoped<Bank.Application.Interfaces.IAccountLifecycleService, Bank.Application.Services.AccountLifecycleService>();
        services.AddScoped<Bank.Application.Interfaces.IAccountLockoutService, Bank.Application.Services.AccountLockoutService>();
        services.AddScoped<Bank.Application.Interfaces.IJointAccountService, Bank.Application.Services.JointAccountService>();

        // Transaction & Payment Services
        services.AddScoped<Bank.Application.Interfaces.ITransactionService, Bank.Application.Services.TransactionService>();
        services.AddScoped<Bank.Application.Interfaces.IBatchService, Bank.Application.Services.BatchService>();
        services.AddScoped<Bank.Application.Interfaces.IRecurringPaymentService, Bank.Application.Services.RecurringPaymentService>();
        services.AddScoped<Bank.Application.Interfaces.IPaymentTemplateService, Bank.Application.Services.PaymentTemplateService>();
        services.AddScoped<Bank.Application.Interfaces.IBeneficiaryService, Bank.Application.Services.BeneficiaryService>();
        services.AddScoped<Bank.Application.Interfaces.IAccountValidationService, Bank.Application.Services.AccountValidationService>();
        services.AddScoped<Bank.Application.Interfaces.ITransferEligibilityService, Bank.Application.Services.TransferEligibilityService>();

        // Statement Services
        services.AddScoped<Bank.Application.Interfaces.IStatementService, Bank.Application.Services.StatementService>();
        services.AddScoped<Bank.Application.Interfaces.IStatementGenerator, Bank.Application.Services.StatementGenerator>();

        // Loan Services
        services.AddScoped<Bank.Application.Interfaces.ILoanService, Bank.Application.Services.LoanService>();
        services.AddScoped<Bank.Application.Interfaces.ILoanInterestCalculationService, Bank.Application.Services.LoanInterestCalculationService>();
        services.AddScoped<Bank.Application.Interfaces.ILoanAnalyticsService, Bank.Application.Services.LoanAnalyticsService>();
        
        // Card Services
        services.AddScoped<Bank.Application.Interfaces.ICardService, Bank.Application.Services.CardService>();
        services.AddScoped<Bank.Application.Interfaces.ICardNetworkService, Bank.Application.Services.CardNetworkService>();
        services.AddScoped<Bank.Application.Interfaces.IPinManagementService, Bank.Application.Services.PinManagementService>();
        
        // Bill Payment Services
        services.AddScoped<Bank.Application.Interfaces.IBillPaymentService, Bank.Application.Services.BillPaymentService>();
        services.AddScoped<Bank.Application.Interfaces.IBillerIntegrationService, Bank.Application.Services.BillerIntegrationService>();
        services.AddScoped<Bank.Application.Interfaces.IPaymentRetryService, Bank.Application.Services.PaymentRetryService>();
        services.AddScoped<Bank.Application.Interfaces.IPaymentReceiptService, Bank.Application.Services.PaymentReceiptService>();
        services.AddScoped<Bank.Application.Interfaces.IBillPresentmentService, Bank.Application.Services.BillPresentmentService>();
        
        // Deposit Services
        services.AddScoped<Bank.Application.Interfaces.IDepositService, Bank.Application.Services.DepositService>();
        services.AddScoped<Bank.Application.Services.IDepositMaturityService, Bank.Application.Services.DepositMaturityService>();
        services.AddScoped<Bank.Application.Services.IDepositWithdrawalService, Bank.Application.Services.DepositWithdrawalService>();
        
        // HTTP Client for external integrations
        services.AddHttpClient<Bank.Application.Services.BillerIntegrationService>();
        
        // Notification Services
        services.AddScoped<Bank.Application.Interfaces.INotificationService, Bank.Application.Services.NotificationService>();
        
        // Background Services
        services.AddHostedService<Bank.Application.Services.LoanBackgroundService>();
        services.AddHostedService<Bank.Application.Services.BillPaymentBackgroundService>();
        services.AddHostedService<Bank.Application.Services.BillerHealthCheckBackgroundService>();
        services.AddHostedService<Bank.Application.Services.DepositBackgroundService>();

        // Financial Calculation Services
        services.AddScoped<Bank.Application.Interfaces.IFeeCalculationService, Bank.Application.Services.FeeCalculationService>();
        services.AddScoped<Bank.Application.Interfaces.IInterestCalculationService, Bank.Application.Services.InterestCalculationService>();

        // Security & Compliance Services
        services.AddScoped<Bank.Application.Interfaces.IFraudDetectionService, Bank.Application.Services.FraudDetectionService>();
        services.AddScoped<Bank.Application.Interfaces.IAuditLogService, Bank.Application.Services.AuditLogService>();
        services.AddScoped<Bank.Application.Interfaces.IAuditEventPublisher, Bank.Application.Services.AuditEventPublisher>();
        services.AddScoped<Bank.Application.Interfaces.IPasswordPolicyService, Bank.Application.Services.PasswordPolicyService>();
        services.AddScoped<Bank.Application.Interfaces.IIpWhitelistService, Bank.Application.Services.IpWhitelistService>();

        // Utility Services
        services.AddScoped<Bank.Application.Interfaces.ITokenGenerationService, Bank.Application.Services.TokenGenerationService>();
        services.AddScoped<Bank.Application.Interfaces.ICalculationService, Bank.Application.Services.CalculationService>();
        services.AddScoped<Bank.Application.Interfaces.IValidationService, Bank.Application.Services.ValidationService>();

        return services;
    }

    /// <summary>
    /// Register all infrastructure services (external integrations)
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Communication Services
        services.AddScoped<Bank.Application.Interfaces.IEmailService, Bank.Infrastructure.Services.EmailService>();
        services.AddScoped<Bank.Application.Interfaces.ISmsService, Bank.Infrastructure.Services.SmsService>();

        // Rate Limiting Service
        services.AddScoped<Bank.Application.Interfaces.IRateLimitingService, Bank.Infrastructure.Services.RateLimitingService>();

        return services;
    }

    /// <summary>
    /// Register CQRS and validation services
    /// </summary>
    public static IServiceCollection AddCqrsServices(this IServiceCollection services)
    {
        // MediatR for CQRS pattern
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Bank.Application.Commands.InitiateTransactionCommand).Assembly));
        
        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Bank.Application.Validators.InitiateTransactionCommandValidator).Assembly);

        return services;
    }

    /// <summary>
    /// Register background job services
    /// </summary>
    public static IServiceCollection AddBackgroundJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Hangfire for background jobs
        services.AddHangfire(config => config
            .UseSqlServerStorage(connectionString));
        services.AddHangfireServer();

        return services;
    }

    /// <summary>
    /// Register API documentation services
    /// </summary>
    public static IServiceCollection AddApiDocumentationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Bank Payment Simulator API",
                Version = "v1",
                Description = "Fintech Payment System Simulator - ACH, WPS, RTGS & Batch Processing"
            });

            // JWT Bearer auth in Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Register CORS policies
    /// </summary>
    public static IServiceCollection AddCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}