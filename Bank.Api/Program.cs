using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Hangfire;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure;
using Bank.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Database
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(connectionString));

// ASP.NET Core Identity
builder.Services.AddIdentity<User, Role>(options =>
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
builder.Services.AddAuthentication(opts => {
    opts.DefaultAuthenticateScheme = "Bearer";
    opts.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options => {
    var jwtSettings = builder.Configuration.GetSection("Jwt");
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

// Unit of Work & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application Services
builder.Services.AddScoped<Bank.Application.Interfaces.IAuthService, Bank.Application.Services.AuthService>();
builder.Services.AddScoped<Bank.Application.Interfaces.IAccountService, Bank.Application.Services.AccountService>();
builder.Services.AddScoped<Bank.Application.Interfaces.ITransactionService, Bank.Application.Services.TransactionService>();
builder.Services.AddScoped<Bank.Application.Interfaces.IBatchService, Bank.Application.Services.BatchService>();

// CQRS / MediatR & FluentValidation
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Bank.Application.Commands.InitiateTransactionCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Bank.Application.Validators.InitiateTransactionCommandValidator).Assembly);

// Hangfire
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

var app = builder.Build();

// Seed roles on startup
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    
    string[] roles = { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new Role { Name = role, Description = $"{role} role" });
        }
    }

    // Seed Admin User
    var adminEmail = "admin@finbank.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new User
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Admin"
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            
            // Seed a default account for the admin
            var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();
            if (!dbContext.Accounts.Any(a => a.UserId == adminUser.Id))
            {
                dbContext.Accounts.Add(new Account
                {
                    UserId = adminUser.Id,
                    AccountNumber = "DE-1000-AD",
                    AccountHolderName = "System Admin",
                    Balance = 50000.00m
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank Simulator API v1");
        options.RoutePrefix = string.Empty; // Swagger UI at root
    });
    app.UseHangfireDashboard();
}
app.UseMiddleware<Bank.Api.Middleware.GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
