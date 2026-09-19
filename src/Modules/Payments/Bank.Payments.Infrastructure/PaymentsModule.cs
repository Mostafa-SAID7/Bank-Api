using Bank.BuildingBlocks.Application.Modules;
using Bank.Payments.Domain;

namespace Bank.Payments.Infrastructure;

/// <summary>
/// Composition root for the Payments bounded context.
/// The host knows this module facade, not the module's internal layers.
/// </summary>
public sealed class PaymentsModule : IModule
{
    public string Name => ModuleMetadata.Name;

    public void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.AddPaymentsInfrastructure();
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // Payments endpoints remain in the legacy API during the pilot migration.
        // They will move to Bank.Payments.Presentation without changing routes.
    }
}