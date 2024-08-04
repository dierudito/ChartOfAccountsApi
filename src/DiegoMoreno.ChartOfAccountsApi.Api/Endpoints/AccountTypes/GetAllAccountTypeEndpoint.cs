using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.AccountTypes;

public class GetAllAccountTypeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
        .WithName("Account Type: Get all")
        .WithSummary("Gets all account types")
        .WithDescription("Gets all account types")
        .Produces<Response<List<AccountTypeResponseDto>>>();

    private static async Task<IResult> HandleAsync(IAccountTypeAppService appService)
    {
        var response = await appService.GetAllAsync();
        return ResponseResult<List<AccountTypeResponseDto>>.CreateResponse(response);
    }
}