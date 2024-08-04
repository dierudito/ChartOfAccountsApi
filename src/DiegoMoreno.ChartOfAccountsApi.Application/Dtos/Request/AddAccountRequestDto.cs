using System.Text.Json.Serialization;

namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
public record AddAccountRequestDto (
    Guid? IdParentAccount, string CodeGroup, string Name, 
    Guid IdAccountType, bool AcceptEntries)
{
    [JsonIgnore]
    public Guid Id { get; init; } = Guid.NewGuid();
}