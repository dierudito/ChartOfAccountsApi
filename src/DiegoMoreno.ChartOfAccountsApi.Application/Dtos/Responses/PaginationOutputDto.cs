using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;

namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
public class PaginationOutputDto(int total, PaginationInputDto pagination)
{
    private const int MAX_PAGINATION_SIZE = 1000000;

    public int Total { get; set; } = total;
    public int? TotalPages
    {
        get
        {
            if (!PageSize.HasValue || PageSize.Value == 0)
            {
                return null;
            }

            return (int)Math.Ceiling(Total / (decimal)PageSize.Value);
        }
    }
    public int? CurrentPage { get; } = pagination?.Page ?? 1;
    public int? PageSize { get; } = 
        pagination == null || pagination?.Size == MAX_PAGINATION_SIZE 
        ? Convert.ToInt32(total) 
        : pagination?.Size;
}
