using DiegoMoreno.ChartOfAccountsApi.Api.Extensions;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
using DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DiegoMoreno.ChartOfAccountsApi.Api.Endpoints.Accounts;

public class SearchAccountEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/search", HandleAsync)
        .WithName("Account: Search")
        .WithSummary("Search a account")
        .WithDescription("Search a account")
        .WithOrder(3)
        .Produces<Response<GetListAccountWithCodeGroupResponseDto>>();

    private static async Task<IResult> HandleAsync(
        IAccountAppService appService,
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = ApiConfigurations.DefaultPageNumber,
        [FromQuery] int pageSize = ApiConfigurations.DefaultPageSize)
    {
        var request = new SearchAccountRequestDto(
            searchTerm,
            new(pageNumber, pageSize));

        var response = await appService.SearchAccountAsync(request);
        return ResponseResult<GetListAccountWithCodeGroupResponseDto>.CreateResponse(response);
    }
}