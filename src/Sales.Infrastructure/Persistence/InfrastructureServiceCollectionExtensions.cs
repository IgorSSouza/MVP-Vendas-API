using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Abstractions.Auth;
using Sales.Application.Abstractions.Health;
using Sales.Infrastructure.Auth;
using Sales.Infrastructure.Health;

namespace Sales.Infrastructure.Persistence;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseHealthChecker, EfDatabaseHealthChecker>();
        services.AddScoped<IGoogleTokenValidator, GoogleIdTokenValidator>();
        services.AddScoped<IAccessTokenService, JwtAccessTokenService>();

        return services;
    }
}
