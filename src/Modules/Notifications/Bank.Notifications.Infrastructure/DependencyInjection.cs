namespace Bank.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsInfrastructure(this IServiceCollection services)
    {
        // This adapter is used until the Notifications schema and its EF Core
        // store are introduced. Application code depends only on the port.
        services.AddSingleton<Bank.Notifications.Application.Notifications.INotificationStore,
            Bank.Notifications.Application.Notifications.InMemoryNotificationStore>();
        services.AddScoped<Bank.Contracts.Notifications.INotificationDispatchContract,
            Bank.Notifications.Application.Notifications.NotificationDispatcher>();
        services.AddSingleton(TimeProvider.System);

        // Email/SMS adapters and the event consumer move here with the schema
        // migration; no module is permitted to access another module's tables.
        return services;
    }
}
