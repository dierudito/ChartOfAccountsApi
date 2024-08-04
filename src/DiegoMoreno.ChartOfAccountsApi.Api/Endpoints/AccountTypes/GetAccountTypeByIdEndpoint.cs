using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.AccountTypes;

public class GetAccountTypeByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{idAccountType:Guid}", HandleAsync)
        .WithName("Account Type: Get Unique")
        .WithSummary("Gets a account type")
        .WithDescription("Gets a account type by id")
        .Produces<Response<AccountTypeResponseDto>> ();

    private static async Task<IResult> HandleAsync(
        IAccountTypeAppService appService,
        Guid idAccountType)
    {
        var response = await appService.GetByIdAsync(new(idAccountType));
        return ResponseResult<AccountTypeResponseDto>.CreateResponse(response);
    }
}
