namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
public record SearchAccountRequestDto(string searchTerm, PaginationInputDto Pagination);