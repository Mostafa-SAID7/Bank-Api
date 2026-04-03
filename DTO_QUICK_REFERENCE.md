# DTO Quick Reference Guide

**Last Updated**: April 3, 2026  
**Status**: ✅ Complete

---

## Quick Navigation

### Finding DTOs by Domain

**Auth DTOs** → `Bank-Api/src/Bank.Application/DTOs/Auth/`
```csharp
using Bank.Application.DTOs.Auth.Core;           // LoginRequestDto, RegisterRequestDto
using Bank.Application.DTOs.Auth.TwoFactor;      // 2FA DTOs
using Bank.Application.DTOs.Auth.Security;       // IP whitelist, password validation
using Bank.Application.DTOs.Auth.Session;        // Session management
```

**Card DTOs** → `Bank-Api/src/Bank.Application/DTOs/Card/`
```csharp
using Bank.Application.DTOs.Card.Core;           // CardDetailsDto, CardDto
using Bank.Application.DTOs.Card.Activation;     // Card activation/issuance
using Bank.Application.DTOs.Card.Transactions;   // Transaction DTOs
using Bank.Application.DTOs.Card.Fees;           // Fee-related DTOs
using Bank.Application.DTOs.Card.Operations;     // Block, PIN, validation, etc.
using Bank.Application.DTOs.Card.Advanced;       // Advanced features
```

**Loan DTOs** → `Bank-Api/src/Bank.Application/DTOs/Loan/`
```csharp
using Bank.Application.DTOs.Loan.Core;           // LoanDetailsDto, LoanApplicationRequestDto
using Bank.Application.DTOs.Loan.Application;    // Credit score, application
using Bank.Application.DTOs.Loan.Approval;       // Approval decision
using Bank.Application.DTOs.Loan.Disbursement;   // Disbursement
using Bank.Application.DTOs.Loan.Repayment;      // Repayment schedule
using Bank.Application.DTOs.Loan.Analytics;      // Analytics and metrics
using Bank.Application.DTOs.Loan.Configuration;  // Configuration and settings
```

**Payment DTOs** → `Bank-Api/src/Bank.Application/DTOs/Payment/`
```csharp
using Bank.Application.DTOs.Payment.Core;        // BillerPaymentRequestDto
using Bank.Application.DTOs.Payment.Beneficiary; // Beneficiary management
using Bank.Application.DTOs.Payment.Biller;      // Biller integration
using Bank.Application.DTOs.Payment.Batch;       // Batch payments
using Bank.Application.DTOs.Payment.Routing;     // Payment routing
using Bank.Application.DTOs.Payment.Receipt;     // Payment receipts
using Bank.Application.DTOs.Payment.Recurring;   // Recurring payments
using Bank.Application.DTOs.Payment.Template;    // Payment templates
```

**Statement DTOs** → `Bank-Api/src/Bank.Application/DTOs/Statement/`
```csharp
using Bank.Application.DTOs.Statement.Core;      // StatementDetailsDto
using Bank.Application.DTOs.Statement.Search;    // Search criteria
using Bank.Application.DTOs.Statement.Summary;   // Summary data
using Bank.Application.DTOs.Statement.Delivery;  // Delivery options
using Bank.Application.DTOs.Statement.Analytics; // Analytics
using Bank.Application.DTOs.Statement.Transaction; // Transaction details
```

**Other Domains**:
```csharp
using Bank.Application.DTOs.Account.*;           // Account management (6 subfolders)
using Bank.Application.DTOs.Deposit.*;           // Deposit products (5 subfolders)
using Bank.Application.DTOs.Transaction.*;       // Transactions (4 subfolders)
using Bank.Application.DTOs.Shared.*;            // Shared DTOs (3 subfolders)
```

---

## Common DTO Patterns

### Authentication Flow
```csharp
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.DTOs.Auth.Security;

// Register
var registerRequest = new RegisterRequestDto { ... };

// Login
var loginRequest = new LoginRequestDto { ... };

// 2FA
using Bank.Application.DTOs.Auth.TwoFactor;
var twoFactorRequest = new GenerateTokenRequestDto { ... };
```

### Loan Application Flow
```csharp
using Bank.Application.DTOs.Loan.Core;
using Bank.Application.DTOs.Loan.Application;
using Bank.Application.DTOs.Loan.Approval;
using Bank.Application.DTOs.Loan.Repayment;

// Apply for loan
var loanRequest = new LoanApplicationRequestDto { ... };

// Check credit score
using Bank.Application.DTOs.Loan.Application;
var creditScore = new CreditScoreDto { ... };

// Get approval
using Bank.Application.DTOs.Loan.Approval;
var approval = new ApprovalDecisionDto { ... };

// View repayment schedule
using Bank.Application.DTOs.Loan.Repayment;
var schedule = new RepaymentScheduleDto { ... };
```

### Payment Processing
```csharp
using Bank.Application.DTOs.Payment.Core;
using Bank.Application.DTOs.Payment.Beneficiary;
using Bank.Application.DTOs.Payment.Receipt;

// Add beneficiary
var beneficiary = new AddBeneficiaryRequestDto { ... };

// Make payment
var payment = new BillerPaymentRequestDto { ... };

// Get receipt
using Bank.Application.DTOs.Payment.Receipt;
var receipt = new PaymentReceiptDto { ... };
```

---

## Folder Structure at a Glance

```
DTOs/
├── Account/          (35 files, 6 subfolders)
├── Auth/             (21 files, 4 subfolders)
├── Card/             (30 files, 6 subfolders)
├── Deposit/          (15 files, 5 subfolders)
├── Loan/             (29 files, 7 subfolders)
├── Payment/          (34 files, 8 subfolders)
├── Shared/           (11 files, 3 subfolders)
├── Statement/        (15 files, 6 subfolders)
└── Transaction/      (5 files, 4 subfolders)

TOTAL: 195 files in 49 subfolders
```

---

## Namespace Pattern

**Pattern**: `Bank.Application.DTOs.{Domain}.{Subfolder}`

**Examples**:
- `Bank.Application.DTOs.Auth.Core`
- `Bank.Application.DTOs.Card.Advanced`
- `Bank.Application.DTOs.Loan.Repayment`
- `Bank.Application.DTOs.Payment.Beneficiary`
- `Bank.Application.DTOs.Statement.Analytics`

---

## Adding New DTOs

### Step 1: Determine Domain and Subfolder
- Choose appropriate domain (Auth, Card, Loan, etc.)
- Choose appropriate subfolder (Core, Advanced, etc.)

### Step 2: Create File
```
Bank-Api/src/Bank.Application/DTOs/{Domain}/{Subfolder}/YourNewDto.cs
```

### Step 3: Set Namespace
```csharp
namespace Bank.Application.DTOs.{Domain}.{Subfolder};

public class YourNewDto
{
    // Properties
}
```

### Step 4: Use in Code
```csharp
using Bank.Application.DTOs.{Domain}.{Subfolder};

// Use YourNewDto
```

---

## Common Mistakes to Avoid

❌ **Wrong**: Using old generic namespace
```csharp
using Bank.Application.DTOs.Loan;  // ❌ WRONG
```

✅ **Right**: Using specific subfolder namespace
```csharp
using Bank.Application.DTOs.Loan.Core;           // ✅ CORRECT
using Bank.Application.DTOs.Loan.Repayment;     // ✅ CORRECT
```

❌ **Wrong**: Putting multiple classes in one file
```csharp
// ❌ WRONG - Multiple classes in one file
public class LoanDto { }
public class LoanDetailsDto { }
```

✅ **Right**: One class per file
```csharp
// ✅ CORRECT - One class per file
// File: LoanDto.cs
public class LoanDto { }

// File: LoanDetailsDto.cs
public class LoanDetailsDto { }
```

---

## IDE Tips

### Visual Studio
1. Use Ctrl+T to open "Go to Type"
2. Type "LoanApplicationRequestDto" to find it
3. IDE will show: `Bank.Application.DTOs.Loan.Core`

### VS Code
1. Use Ctrl+P to open "Go to File"
2. Type "LoanApplicationRequestDto.cs"
3. File path shows the namespace structure

### Rider
1. Use Ctrl+Shift+A to open "Go to Action"
2. Type "LoanApplicationRequestDto"
3. Navigate directly to the file

---

## Useful Commands

### Find all DTOs in a domain
```bash
Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs/Loan" -Recurse -Filter "*.cs"
```

### Count DTOs by domain
```bash
$domains = @("Auth", "Card", "Loan", "Payment", "Statement", "Transaction", "Shared", "Deposit", "Account")
foreach ($domain in $domains) {
    $count = @(Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs/$domain" -Recurse -Filter "*.cs").Count
    Write-Host "$domain`: $count files"
}
```

### Verify all DTOs have correct namespaces
```bash
Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs" -Recurse -Filter "*.cs" | 
    ForEach-Object { 
        $content = Get-Content $_.FullName
        if ($content -notmatch "namespace Bank.Application.DTOs") {
            Write-Host "ERROR: $($_.FullName) has incorrect namespace"
        }
    }
```

---

## Related Documentation

- **[DTO_MIGRATION_COMPLETE.md](DTO_MIGRATION_COMPLETE.md)** - Complete migration report
- **[DTO_REORGANIZATION_VERIFICATION.md](DTO_REORGANIZATION_VERIFICATION.md)** - Verification details
- **[ALL_DTOS_ORGANIZATION_STRUCTURE.md](ALL_DTOS_ORGANIZATION_STRUCTURE.md)** - Full structure reference
- **[IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md)** - Implementation guide

---

## Quick Links

| Domain | Subfolders | Files | Path |
|--------|-----------|-------|------|
| Account | 6 | 35 | `DTOs/Account/` |
| Auth | 4 | 21 | `DTOs/Auth/` |
| Card | 6 | 30 | `DTOs/Card/` |
| Deposit | 5 | 15 | `DTOs/Deposit/` |
| Loan | 7 | 29 | `DTOs/Loan/` |
| Payment | 8 | 34 | `DTOs/Payment/` |
| Shared | 3 | 11 | `DTOs/Shared/` |
| Statement | 6 | 15 | `DTOs/Statement/` |
| Transaction | 4 | 5 | `DTOs/Transaction/` |

---

**Status**: ✅ Ready to Use  
**Last Updated**: April 3, 2026
