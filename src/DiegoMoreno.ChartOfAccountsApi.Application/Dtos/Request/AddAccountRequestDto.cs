namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
public record AddAccountRequestDto (
    Guid? IdParentAccount, string CodeGroup, string Name, 
    Guid IdAccountType, bool AcceptEntries)
{
    public Guid Id { get; init; } = Guid.NewGuid();
}