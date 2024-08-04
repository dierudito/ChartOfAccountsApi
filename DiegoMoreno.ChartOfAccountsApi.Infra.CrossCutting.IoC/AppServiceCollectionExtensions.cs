using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Services;
using DiegoMoreno.ChartOfAccountsApi.Domain.Services;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC.Configurations;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC;
public static class AppServiceCollectionExtensions
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services) =>
        services
        .AddAutoMapper()
        .AddRepositories()
        .AddDomainServices();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
        .AddTransient<IAccountRepository, AccountRepository>()
        .AddTransient<IAccountTypeRepository, AccountTypeRepository>();

    private static IServiceCollection AddDomainServices(this IServiceCollection services) =>
        services
        .AddTransient<IAccountService, AccountService>();

}