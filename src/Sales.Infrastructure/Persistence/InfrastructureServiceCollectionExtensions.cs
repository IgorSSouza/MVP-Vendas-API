using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Abstractions.Health;
using Sales.Infrastructure.Health;

namespace Sales.Infrastructure.Persistence;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseHealthChecker, EfDatabaseHealthChecker>();

        return services;
    }
}
