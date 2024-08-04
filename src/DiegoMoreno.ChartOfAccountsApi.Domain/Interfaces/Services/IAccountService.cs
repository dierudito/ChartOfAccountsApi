using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Services;
public interface IAccountService
{
    Task<Account> AddAsync(Account account);

    Task<Account> UpdateAsync(Account account);

    Task DeleteAsync(Guid accountId);

    Task<string> CalculateNextCodeAsync(Guid? currentAccountId);

    Task<(string? suggestedCode, Guid? account)> CalculateNextNewParentCodeAsync(Guid currentAccountId);

    Task<string> GetCodeGroupAsync(Account account);
}