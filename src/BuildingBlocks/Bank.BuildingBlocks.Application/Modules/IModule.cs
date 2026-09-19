namespace Bank.BuildingBlocks.Application.Modules;

/// <summary>
/// Entry point for a bounded context in the modular monolith.
/// Modules register their own services and endpoint mappings without exposing
/// their internal application or infrastructure types to another module.
/// </summary>
public interface IModule
{
    string Name { get; }

    void AddServices(IServiceCollection services, IConfiguration configuration);

    void MapEndpoints(IEndpointRouteBuilder endpoints);
}