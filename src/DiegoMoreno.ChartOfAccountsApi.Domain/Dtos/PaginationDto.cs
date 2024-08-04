namespace DiegoMoreno.ChartOfAccountsApi.Domain.Dtos;
public class PaginationDto()
{
    private const int ITEMS_PER_PAGE = 10;

    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value;
    }

    private int _size = ITEMS_PER_PAGE;
    public int Size
    {
        get => _size;
        set => _size = value <= 0 ? ITEMS_PER_PAGE : value;
    }

    public int Skip() { return ITEMS_PER_PAGE * (_page - 1); }
    public int Take() { return Size; }
}