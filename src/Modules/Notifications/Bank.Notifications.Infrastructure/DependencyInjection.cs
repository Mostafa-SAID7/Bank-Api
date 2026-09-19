namespace Bank.Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationsInfrastructure(this IServiceCollection services)
    {
        // Email/SMS adapters and the event consumer will move here in the
        // Notifications pilot.
        return services;
    }
}