﻿namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
public record GetListAccountWithCodeGroupRequestDto(bool OnlyParent, PaginationInputDto Pagination);