namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos;
public record AccountWithCodeGroupDto(Guid Id, string CodeGroup, string Name, Guid IdAccountType, string typeName, bool acceptEntries);