using DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;
using DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.AccountTypes;
using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints;


public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup($"v1/{ApiConfigurations.RouteAccount}")
            .WithTags("Accounts")
            .MapEndpoint<CreateAccountEndpoint>()
            .MapEndpoint<DeleteAccountEndpoint>()
            .MapEndpoint<GetListAccountWithCodeGroupEndpoint>()
            .MapEndpoint<GetNextAccountCodeEndpoint>()
            .MapEndpoint<SearchAccountEndpoint>();

        endpoints.MapGroup($"v1/{ApiConfigurations.RouteAccountType}")
            .WithTags("Account Types")
            .MapEndpoint<GetAccountTypeByIdEndpoint>()
            .MapEndpoint<GetAllAccountTypeEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}