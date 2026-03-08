using Bank.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add controllers with JSON configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// === SERVICE REGISTRATIONS (Organized by Concern) ===

// 1. Database & Data Access Layer
builder.Services.AddDatabaseServices(builder.Configuration);

// 2. Caching & Session Management
builder.Services.AddCachingServices(builder.Configuration);

// 3. Authentication & Authorization
builder.Services.AddAuthenticationServices(builder.Configuration);

// 4. Repository Layer (Data Access)
builder.Services.AddRepositoryServices();

// 5. Application Services (Business Logic)
builder.Services.AddApplicationServices();

// 6. Infrastructure Services (External Integrations)
builder.Services.AddInfrastructureServices();

// 7. CQRS & Validation
builder.Services.AddCqrsServices();

// 8. Background Jobs
builder.Services.AddBackgroundJobServices(builder.Configuration);

// 9. API Documentation
builder.Services.AddApiDocumentationServices();

// 10. CORS Policies
builder.Services.AddCorsServices();

var app = builder.Build();

// === DATA SEEDING ===
await app.SeedInitialDataAsync();

// === MIDDLEWARE PIPELINE (Order Matters) ===

// 1. Development-specific middleware
app.ConfigureDevelopmentMiddleware();

// 2. Security & audit middleware
app.ConfigureSecurityMiddleware();

// 3. Standard ASP.NET Core middleware
app.ConfigureStandardMiddleware();

app.Run();

// Make Program class accessible for testing
public partial class Program { }
