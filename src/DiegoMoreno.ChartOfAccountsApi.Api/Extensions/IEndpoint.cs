﻿namespace DiegoMoreno.ChartOfAccountsApi.Api.Extensions;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder app);
}