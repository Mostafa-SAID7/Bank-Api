using Bank.CoreBanking.Application;
using Bank.CoreBanking.Presentation.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bank.CoreBanking.Presentation.Endpoints;

/// <summary>
/// Core Banking endpoints
/// Currently placeholder - routes will be fully implemented in Phase 3b
/// </summary>
public static class CoreBankingEndpoints
{
    public static void MapCoreBankingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/core-banking")
            .WithName("CoreBanking");

        // Account endpoints (deferred to Phase 3b)
        group.MapPost("/accounts", CreateAccount)
            .WithName("CreateAccount");

        group.MapGet("/accounts/{id}", GetAccount)
            .WithName("GetAccount");

        group.MapGet("/accounts/customer/{customerId}", GetCustomerAccounts)
            .WithName("GetCustomerAccounts");

        // Transaction endpoints (deferred to Phase 3b)
        group.MapGet("/transactions/{id}", GetTransaction)
            .WithName("GetTransaction");

        group.MapGet("/accounts/{accountId}/transactions", GetAccountTransactions)
            .WithName("GetAccountTransactions");
    }

    private static IResult CreateAccount(CreateAccountRequest request, IAccountRepository accountRepository)
    {
        // TODO: Implement in Phase 3b
        return Results.StatusCode(501); // Not Implemented
    }

    private static IResult GetAccount(Guid id, IAccountRepository accountRepository)
    {
        // TODO: Implement in Phase 3b
        return Results.StatusCode(501); // Not Implemented
    }

    private static IResult GetCustomerAccounts(Guid customerId, IAccountRepository accountRepository)
    {
        // TODO: Implement in Phase 3b
        return Results.StatusCode(501); // Not Implemented
    }

    private static IResult GetTransaction(Guid id, ITransactionRepository transactionRepository)
    {
        // TODO: Implement in Phase 3b
        return Results.StatusCode(501); // Not Implemented
    }

    private static IResult GetAccountTransactions(Guid accountId, ITransactionRepository transactionRepository)
    {
        // TODO: Implement in Phase 3b
        return Results.StatusCode(501); // Not Implemented
    }
}
