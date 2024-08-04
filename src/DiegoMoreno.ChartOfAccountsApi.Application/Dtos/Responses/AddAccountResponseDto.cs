namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
public record AddAccountResponseDto(Guid Id, Guid? IdParentAccount, int Code, string Name, Guid IdAccountType, bool AcceptEntries);