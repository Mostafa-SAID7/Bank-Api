using Microsoft.Data.SqlClient;

var connectionString = "Server=db43962.public.databaseasp.net;Database=db43962;User Id=db43962;Password=D!m4kG9_%6eW;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

try
{
    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    Console.WriteLine("✅ Database connection successful!");
    
    // Test a simple query
    using var command = new SqlCommand("SELECT 1", connection);
    var result = await command.ExecuteScalarAsync();
    Console.WriteLine($"✅ Query executed successfully. Result: {result}");
    
    Console.WriteLine("🎉 Database connectivity test completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}