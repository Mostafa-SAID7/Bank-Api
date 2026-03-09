using Microsoft.Data.SqlClient;

var connectionString = "Server=db43962.public.databaseasp.net;Database=db43962;User Id=db43962;Password=D!m4kG9_%6eW;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

try
{
    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    Console.WriteLine("✅ Database connection successful!");
    
    // Check what tables exist
    using var command = new SqlCommand(@"
        SELECT TABLE_NAME 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_TYPE = 'BASE TABLE' 
        ORDER BY TABLE_NAME", connection);
    
    using var reader = await command.ExecuteReaderAsync();
    
    Console.WriteLine("\n📋 Existing tables in database:");
    var tableCount = 0;
    while (await reader.ReadAsync())
    {
        Console.WriteLine($"  - {reader.GetString("TABLE_NAME")}");
        tableCount++;
    }
    
    Console.WriteLine($"\n✅ Total tables found: {tableCount}");
    
    if (tableCount == 0)
    {
        Console.WriteLine("🔍 Database is empty - ready for migrations!");
    }
    else
    {
        Console.WriteLine("🔍 Database has existing tables - some migrations may have been applied.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}