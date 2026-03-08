# Entity Relationship Diagram

## Database Schema

This document describes the database schema and relationships for the Bank Management System.

### Entities

#### Users
- Primary key: UserId
- Attributes: Username, Email, PasswordHash, CreatedAt, UpdatedAt

#### Accounts
- Primary key: AccountId
- Foreign key: UserId
- Attributes: AccountNumber, AccountType, Balance, CreatedAt, UpdatedAt

#### Transactions
- Primary key: TransactionId
- Foreign keys: FromAccountId, ToAccountId
- Attributes: Amount, TransactionType, Description, Timestamp, Status

### Relationships

- Users (1) → (N) Accounts
- Accounts (1) → (N) Transactions (as source)
- Accounts (1) → (N) Transactions (as destination)

## ERD Diagram

```
[Users] 1---N [Accounts] 1---N [Transactions]
```

*Note: Add visual ERD diagram here*