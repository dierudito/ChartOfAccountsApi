using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;

public class GetNextAccountCodeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/nextCode/{idParentAccount:Guid}", HandleAsync)
        .WithName("Account: Get next code")
        .WithSummary("Gets a suggested next code")
        .WithDescription("Gets a suggested next code form a new account")
        .WithOrder(2)
        .Produces<Response<NextAccountCodeResponseDto>>();

    private static async Task<IResult> HandleAsync(IAccountAppService appService, Guid? idParentAccount)
    {
        var response = await appService.GetSuggestedCodeAsync(new(idParentAccount));
        return ResponseResult<NextAccountCodeResponseDto>.CreateResponse(response);
    }
}
