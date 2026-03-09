using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Bank.Infrastructure.Data;

var connectionString = "Server=db43977.public.databaseasp.net;Database=db43977;User Id=db43977;Password=3Sp-m9?A7+Kt;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

var options = new DbContextOptionsBuilder<BankDbContext>()
    .UseSqlServer(connectionString, sqlOptions => 
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    })
    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
    .Options;

try
{
    using var context = new BankDbContext(options);
    
    Console.WriteLine("Testing database connection with retry logic...");
    
    // Test connection
    await context.Database.OpenConnectionAsync();
    Console.WriteLine("✅ Database connection successful!");
    
    // Check if database exists
    var canConnect = await context.Database.CanConnectAsync();
    Console.WriteLine($"✅ Can connect to database: {canConnect}");
    
    // Check pending migrations
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    Console.WriteLine($"📋 Pending migrations: {pendingMigrations.Count()}");
    
    if (pendingMigrations.Any())
    {
        Console.WriteLine("Pending migrations:");
        foreach (var migration in pendingMigrations)
        {
            Console.WriteLine($"  - {migration}");
        }
        
        // Apply migrations
        Console.WriteLine("Applying migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied successfully!");
    }
    else
    {
        Console.WriteLine("✅ No pending migrations - database is up to date!");
    }
    
    // Test a simple query
    var userCount = await context.Users.CountAsync();
    Console.WriteLine($"✅ Current user count: {userCount}");
    
    Console.WriteLine("🎉 Database setup completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    
    Console.WriteLine("💡 Suggestions:");
    Console.WriteLine("1. Check if the database server is accessible");
    Console.WriteLine("2. Verify the connection string credentials");
    Console.WriteLine("3. Ensure the database exists and is not in single-user mode");
    Console.WriteLine("4. Try again in a few minutes if this is a transient issue");
}
