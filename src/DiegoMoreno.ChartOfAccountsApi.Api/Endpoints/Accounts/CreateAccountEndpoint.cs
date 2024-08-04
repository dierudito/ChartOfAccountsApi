using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;

public class CreateAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
        .WithName("Account: Create")
        .WithSummary("Creates a new account")
        .WithDescription("Creates a new account")
        .WithOrder(1)
        .Produces<Response<AddAccountResponseDto>>();

    private static async Task<IResult> HandleAsync(IAccountAppService appService, AddAccountRequestDto request)
    {
        var response = await appService.AddAsync(request);
        return ResponseResult<AddAccountResponseDto>.CreateResponse(response);
    }
}
