using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Extensions;

public static class BuildExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        ApiConfigurations.ConncetionString =
            builder.Configuration.GetConnectionString("ChartOfAccountsApiDbSqlServer") ?? string.Empty;
        ApiConfigurations.BackendUrl =
            builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
    }
}
