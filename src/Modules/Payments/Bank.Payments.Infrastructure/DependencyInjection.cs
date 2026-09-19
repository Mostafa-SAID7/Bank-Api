namespace Bank.Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services)
    {
        // Persistence and outbox registrations will be moved here with the
        // Payments pilot. No legacy service is registered from this project.
        return services;
    }
}