using AutoMapper;
using DiegoMoreno.ChartOfAccountsApi.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.IoC.Configurations;
public static class AutoMapperConfig
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AllowNullDestinationValues = true;

            mc.AddProfile(new AccountTypeMapper());
            mc.AddProfile(new AccountMapper());
        });
        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}
