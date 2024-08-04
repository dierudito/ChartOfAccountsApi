using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;

public class GetListAccountWithCodeGroupEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/listWithCodeGroup", HandleAsync)
        .WithName("Account: Get a list with code group")
        .WithSummary("Gets a list with code group")
        .WithDescription("Gets a list with code group")
        .WithOrder(3)
        .Produces<Response<GetListAccountWithCodeGroupResponseDto>>();

    private static async Task<IResult> HandleAsync(
        IAccountAppService appService,
        [FromQuery] bool onlyParents = false,
        [FromQuery] int pageNumber = ApiConfigurations.DefaultPageNumber,
        [FromQuery] int pageSize = ApiConfigurations.DefaultPageSize)
    {
        var request = new GetListAccountWithCodeGroupRequestDto(
            onlyParents,
            new(pageNumber, pageSize));

        var response = await appService.GetListAccountWithCodeGroupAsync(request);
        return ResponseResult<GetListAccountWithCodeGroupResponseDto>.CreateResponse(response);
    }
}
