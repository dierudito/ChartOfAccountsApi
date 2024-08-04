using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;

public class DeleteAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{idAccount:Guid}", HandleAsync)
        .WithName("Account: Delete")
        .WithSummary("Delete a account")
        .WithDescription("Delete a account")
        .WithOrder(3)
        .Produces<Response<bool>>();

    private static async Task<IResult> HandleAsync(IAccountAppService appService, Guid idAccount)
    {
        var response = await appService.DeleteAsync(new(idAccount));
        return ResponseResult<bool>.CreateResponse(response);
    }
}
