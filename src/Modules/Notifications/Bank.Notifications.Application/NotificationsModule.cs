using Bank.BuildingBlocks.Application.Modules;
using Bank.Notifications.Domain;

namespace Bank.Notifications.Application;

public sealed class NotificationsModule : IModule
{
    public string Name => ModuleMetadata.Name;

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Notifications is event-driven and currently has no public endpoints.
    }
}