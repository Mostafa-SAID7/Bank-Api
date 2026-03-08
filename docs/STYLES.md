# Coding Style Guide

## C# Coding Standards

### Naming Conventions

#### Classes and Methods
```csharp
// Use PascalCase for classes, methods, and properties
public class AccountService
{
    public async Task<Account> GetAccountAsync(int accountId)
    {
        // Implementation
    }
}
```

#### Variables and Parameters
```csharp
// Use camelCase for local variables and parameters
public void ProcessTransaction(decimal transactionAmount, string accountNumber)
{
    var currentBalance = GetCurrentBalance();
    // Implementation
}
```

#### Constants
```csharp
// Use UPPER_CASE for constants
public const string DEFAULT_CURRENCY = "USD";
public const int MAX_TRANSACTION_AMOUNT = 10000;
```

### Code Organization

#### File Structure
- One class per file
- File name matches class name
- Organize using statements alphabetically
- Remove unused using statements

#### Method Structure
```csharp
public async Task<TransactionResult> ProcessTransactionAsync(
    TransactionRequest request,
    CancellationToken cancellationToken = default)
{
    // Validation
    if (request == null)
        throw new ArgumentNullException(nameof(request));

    // Business logic
    var result = await _transactionService.ProcessAsync(request, cancellationToken);
    
    // Return result
    return result;
}
```

### Documentation Standards

#### XML Documentation
```csharp
/// <summary>
/// Processes a financial transaction between accounts
/// </summary>
/// <param name="request">The transaction request details</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>The transaction result</returns>
/// <exception cref="ArgumentNullException">Thrown when request is null</exception>
public async Task<TransactionResult> ProcessTransactionAsync(
    TransactionRequest request,
    CancellationToken cancellationToken = default)
```

#### Comments
```csharp
// Use single-line comments for brief explanations
var balance = account.Balance; // Current account balance

/* Use multi-line comments for complex explanations
   that require multiple lines to describe the logic
   or business rules being implemented */
```

### Error Handling

#### Exception Handling
```csharp
try
{
    var result = await ProcessTransactionAsync(request);
    return Ok(result);
}
catch (ValidationException ex)
{
    return BadRequest(ex.Message);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error processing transaction");
    return StatusCode(500, "Internal server error");
}
```

### LINQ and Collections

#### Preferred LINQ Style
```csharp
// Use method syntax for simple operations
var activeAccounts = accounts.Where(a => a.IsActive).ToList();

// Use query syntax for complex operations
var accountSummary = from account in accounts
                    join user in users on account.UserId equals user.Id
                    where account.Balance > 1000
                    select new AccountSummary
                    {
                        AccountNumber = account.Number,
                        UserName = user.Name,
                        Balance = account.Balance
                    };
```

### Async/Await Guidelines

```csharp
// Always use ConfigureAwait(false) in library code
var result = await _repository.GetAccountAsync(id).ConfigureAwait(false);

// Use cancellation tokens for long-running operations
public async Task<List<Transaction>> GetTransactionsAsync(
    int accountId,
    CancellationToken cancellationToken = default)
{
    return await _repository.GetTransactionsAsync(accountId, cancellationToken);
}
```

## Code Quality Tools

### EditorConfig
```ini
root = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = crlf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true
```

### Recommended Extensions
- StyleCop.Analyzers
- Microsoft.CodeAnalysis.FxCopAnalyzers
- SonarAnalyzer.CSharp