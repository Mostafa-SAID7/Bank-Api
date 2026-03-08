namespace Bank.Api.DTOs;

// Account
public record CreateAccountRequest(string AccountHolderName);
public record UpdateAccountRequest(string AccountHolderName);
