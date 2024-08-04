namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
public record NextAccountCodeResponseDto (string SuggestedCode, Guid? NewParent = null);