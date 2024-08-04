using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Contexts.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC.Configurations;

public static class DatabaseConfig
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services) =>
        services
        .AddEntityFrameworkConfiguration();


    private static IServiceCollection AddEntityFrameworkConfiguration(this IServiceCollection services) =>
        services
        .AddDbContext<ChartOfAccountsApiDbContext>(options =>
        {
            options.UseSqlServer(ApiConfigurations.ConncetionString);
#if (DEBUG)
            options.EnableSensitiveDataLogging();
#endif
        });
}
