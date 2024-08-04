using DiegoMoreno.ChartOfAccountsApi.Application.AppServices;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC.Configurations;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services) =>
        services
        .AddDatabaseConfiguration()
        .AddCrossOrigin()
        .AddDocumentation()
        .ResolveDependencies()
        .AddAppServices();

    private static IServiceCollection AddAppServices(this IServiceCollection services) =>
        services
        .AddTransient<IAccountAppService, AccountAppService>()
        .AddTransient<IAccountTypeAppService, AccountTypeAppService>();

    public static IServiceCollection AddDocumentation(this IServiceCollection services) =>
        services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });

    public static IServiceCollection AddCrossOrigin(this IServiceCollection services) =>
        services.AddCors(
            options => options.AddPolicy(
                ApiConfigurations.CorsPolicyName,
            policy => policy
                .WithOrigins([
                    ApiConfigurations.BackendUrl,
                    ApiConfigurations.FrontendUrl
                    ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                ));
}