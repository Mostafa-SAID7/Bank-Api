using Microsoft.EntityFrameworkCore;

namespace Bank.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<Data.NotificationsDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(NotificationsModule).Assembly.GetName().Name)));

        services.AddScoped<Bank.Notifications.Application.Notifications.INotificationStore,
            Data.EfNotificationStore>();
        services.AddScoped<Bank.Notifications.Application.Notifications.INotificationReadStore,
            Data.EfNotificationStore>();
        services.AddScoped<Bank.Contracts.Notifications.INotificationDispatchContract,
            Bank.Notifications.Application.Notifications.NotificationDispatcher>();
        services.AddScoped<Bank.Contracts.Notifications.INotificationManagementContract,
            Bank.Notifications.Application.Notifications.NotificationManagementService>();
        services.AddSingleton(TimeProvider.System);

        // Email/SMS adapters and the event consumer move here next; no module
        // is permitted to access another module's tables.
        return services;
    }
}
