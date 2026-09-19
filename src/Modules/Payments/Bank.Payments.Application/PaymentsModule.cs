using Bank.BuildingBlocks.Application.Modules;
using Bank.Payments.Domain;

namespace Bank.Payments.Application;

public sealed class PaymentsModule : IModule
{
    public string Name => ModuleMetadata.Name;

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Payments endpoints remain in the legacy API during the pilot migration.
        // They will move to Bank.Payments.Presentation without changing routes.
    }
}