using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetAiOps.Application.Auth;
using NetAiOps.Application.Dashboard;
using NetAiOps.Application.Users;
using NetAiOps.Infrastructure.Auth;
using NetAiOps.Application.Operations;
using NetAiOps.Infrastructure.Operations;
using NetAiOps.Infrastructure.Persistence;

namespace NetAiOps.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddDbContext<NetAiOpsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IOperationsService, OperationsService>();
        services.AddHostedService<OutboxWorker>();

        return services;
    }
}
