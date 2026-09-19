using Bank.BuildingBlocks.Application.Modules;
using Bank.Notifications.Domain;

namespace Bank.Notifications.Infrastructure;

/// <summary>
/// Composition root for the Notifications bounded context.
/// The host knows this module facade, not the module's internal layers.
/// </summary>
public sealed class NotificationsModule : IModule
{
    public string Name => ModuleMetadata.Name;

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.AddNotificationsInfrastructure();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Notifications is event-driven and currently has no public endpoints.
    }
}