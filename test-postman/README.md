# Postman Test Collection

This folder contains the Postman collection and environment for manually testing all FinBank API endpoints.

## Files

| File | Description |
|---|---|
| `Bank-API-Complete-Collection.postman_collection.json` | Full collection — 70 endpoints across 8 domain folders |
| `Bank-API-Environment.postman_environment.json` | Environment variables (base URL, auth tokens) |

## How to Import

1. Open **Postman**
2. Click **Import** (top left)
3. Drag and drop **both** JSON files into the import dialog, or select them via file browser
4. The collection `Bank API - Complete Test Collection` and environment `Bank API - Local Development` will appear

## How to Use

1. Select the **`Bank API - Local Development`** environment from the environment dropdown (top right in Postman)
2. Start the API locally:
   ```bash
   dotnet run --project src/Bank.Host/Bank.Host.csproj
   ```
   API runs at `http://localhost:5000`
3. Run the **Authentication** folder first — it sets the `{{accessToken}}` variable automatically via a test script
4. All other folders use `{{accessToken}}` in their Authorization headers

## Coverage

| Folder | Endpoints |
|---|---|
| Authentication & Sessions | Register, Login, Refresh, Logout, 2FA |
| Account Management | Create, Get, Update, Close |
| Card Operations | Issue, Block, Unblock, PIN change |
| Loan Management | Apply, Approve, Repay, Statement |
| Deposits & Savings | Open, Redeem, Interest history |
| Payments & Transfers | Transfer, Bill pay, Beneficiaries |
| Transactions | History, Search, Export |
| Admin Operations | User management, System config |

## Swagger Alternative

For quick endpoint exploration without Postman, Swagger UI is available at:
```
http://localhost:5000/swagger
```
