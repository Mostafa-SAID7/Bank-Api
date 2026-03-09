using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Bank.TestUtilities;

/// <summary>
/// Simple utility to test database connectivity and diagnose connection issues
/// </summary>
public static class ConnectionTestUtility
{
    private static readonly string ConnectionString = 
        "Server=db43977.public.databaseasp.net;Database=db43977;User Id=db43977;Password=3Sp-m9?A7+Kt;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

    public static async Task<bool> TestConnectionAsync()
    {
        Console.WriteLine("🔍 Testing database connection...");
        Console.WriteLine($"📡 Server: db43977.public.databaseasp.net");
        Console.WriteLine($"🗄️ Database: db43977");
        Console.WriteLine();

        try
        {
            using var connection = new SqlConnection(ConnectionString);
            
            var stopwatch = Stopwatch.StartNew();
            await connection.OpenAsync();
            stopwatch.Stop();
            
            Console.WriteLine($"✅ Connection successful! ({stopwatch.ElapsedMilliseconds}ms)");
            Console.WriteLine($"📊 Server Version: {connection.ServerVersion}");
            Console.WriteLine($"🏢 Database: {connection.Database}");
            Console.WriteLine($"🔗 Connection State: {connection.State}");
            
            // Test a simple query
            using var command = new SqlCommand("SELECT GETDATE() as CurrentTime, @@VERSION as Version", connection);
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                Console.WriteLine($"⏰ Server Time: {reader["CurrentTime"]}");
                Console.WriteLine($"📋 SQL Server: {reader["Version"]?.ToString()?.Split('\n')[0]}");
            }
            
            return true;
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine($"❌ SQL Server Error: {sqlEx.Message}");
            Console.WriteLine($"🔢 Error Number: {sqlEx.Number}");
            Console.WriteLine($"📊 Severity: {sqlEx.Class}");
            Console.WriteLine($"🏷️ State: {sqlEx.State}");
            
            switch (sqlEx.Number)
            {
                case 2:
                    Console.WriteLine("💡 Network timeout - server may be unreachable");
                    break;
                case 18:
                    Console.WriteLine("💡 Login failed - check credentials");
                    break;
                case 64:
                    Console.WriteLine("💡 Network name no longer available - connectivity issue");
                    break;
                case 53:
                    Console.WriteLine("💡 Server not found - check server name and network");
                    break;
                default:
                    Console.WriteLine("💡 Check connection string and server availability");
                    break;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.Message}");
            Console.WriteLine($"🔍 Exception Type: {ex.GetType().Name}");
            return false;
        }
    }

    public static async Task RunDiagnosticsAsync()
    {
        Console.WriteLine("🔧 Running database connection diagnostics...");
        Console.WriteLine("=" * 50);
        
        // Test basic connectivity
        var isConnected = await TestConnectionAsync();
        
        if (!isConnected)
        {
            Console.WriteLine();
            Console.WriteLine("🛠️ Troubleshooting suggestions:");
            Console.WriteLine("1. Check internet connectivity");
            Console.WriteLine("2. Verify server name: db43977.public.databaseasp.net");
            Console.WriteLine("3. Ensure database service is running");
            Console.WriteLine("4. Check firewall settings");
            Console.WriteLine("5. Try connecting with SQL Server Management Studio");
            Console.WriteLine("6. Contact your database provider if issues persist");
            Console.WriteLine();
            Console.WriteLine("📝 Manual migration option:");
            Console.WriteLine("   Run: bank-backend/Bank.Infrastructure/improved-migration.sql");
            Console.WriteLine("   In your database management tool");
        }
        
        Console.WriteLine("=" * 50);
    }
}

// Simple console app entry point for testing
public class Program
{
    public static async Task Main(string[] args)
    {
        await ConnectionTestUtility.RunDiagnosticsAsync();
        
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}