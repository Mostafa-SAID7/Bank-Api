namespace Bank.BuildingBlocks.Application.Modules;

public static class ModuleRegistrationExtensions
{
    public static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration,
        params IModule[] modules)
    {
        foreach (var module in modules)
        {
            module.AddServices(services, configuration);
        }

        return services;
    }

    public static WebApplication MapModules(
        this WebApplication app,
        params IModule[] modules)
    {
        foreach (var module in modules)
        {
            module.MapEndpoints(app);
        }

        return app;
    }
}