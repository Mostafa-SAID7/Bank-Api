using Microsoft.EntityFrameworkCore;
using Bank.Infrastructure.Data;

var connectionString = "Server=db43962.public.databaseasp.net;Database=db43962;User Id=db43962;Password=D!m4kG9_%6eW;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

var options = new DbContextOptionsBuilder<BankDbContext>()
    .UseSqlServer(connectionString)
    .Options;

try
{
    using var context = new BankDbContext(options);
    
    Console.WriteLine("Testing database connection...");
    
    // Test connection
    await context.Database.OpenConnectionAsync();
    Console.WriteLine("✅ Database connection successful!");
    
    // Check if database exists
    var canConnect = await context.Database.CanConnectAsync();
    Console.WriteLine($"✅ Can connect to database: {canConnect}");
    
    // Apply migrations
    Console.WriteLine("Applying migrations...");
    await context.Database.MigrateAsync();
    Console.WriteLine("✅ Migrations applied successfully!");
    
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
}