using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Services;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Services;
public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    public async Task<Account> AddAsync(Account account) => await accountRepository.AddAsync(account);

    public async Task<Account> UpdateAsync(Account account) => await accountRepository.UpdateAsync(account);

    public async Task DeleteAsync(Guid accountId) => await accountRepository.DeleteAsync(accountId);

    public async Task<string> CalculateNextCodeAsync(Guid? currentAccountId)
    {
        if (!currentAccountId.HasValue) return "1";

        var currentAccount = await accountRepository.GetByIdAsync(currentAccountId.Value);
        if (currentAccount == null) return "1";

        return await DefineCodeGroupAsync(currentAccount);
    }

    public async Task<(string? suggestedCode, Guid? account)> CalculateNextNewParentCodeAsync(Guid currentAccountId)
    {
        Account? currentAccount;

        do
        {
            currentAccount = await accountRepository.GetByIdAsync(currentAccountId);

            if (!currentAccount!.IdParentAccount.HasValue) continue;
            var codeGroup = await CalculateNextCodeAsync(currentAccount.IdParentAccount);

            if (!string.IsNullOrEmpty(codeGroup)) return (codeGroup, currentAccount.IdParentAccount);
            currentAccountId = currentAccount.IdParentAccount.Value;

        } while (currentAccount.IdParentAccount.HasValue);

        return (string.Empty, null);
    }
    
    public async Task<string> GetCodeGroupAsync(Account account)
    {
        var parentsAccount = await GetAllParentsLevelsAsync(account);
        parentsAccount.Reverse();

        var parentsCodeGroup = string.Join(".", parentsAccount.Select(level => level.Code));

        var codeGroup = !string.IsNullOrWhiteSpace(parentsCodeGroup) ? $"{parentsCodeGroup}." : "";
        codeGroup += account.Code.ToString();

        return codeGroup;
    }

    private async Task<string> DefineCodeGroupAsync(Account account)
    {
        var childAccount = await GetTheLastChildOfTheNextLevelAsync(account);

        string childrenCodeGroup = DefineChildCodeGroup(childAccount);

        if (string.IsNullOrWhiteSpace(childrenCodeGroup)) return string.Empty;

        var codeGroup = await GetCodeGroupAsync(account);
        codeGroup += $".{childrenCodeGroup}";

        return codeGroup;
    }

    private async Task<Account?> GetTheLastChildOfTheNextLevelAsync(Account parentAccount)
    {
        var childrenAccount = await accountRepository.GetAccountChildrenAsync(parentAccount.Id);

        if (childrenAccount == null || !childrenAccount.Any()) return null;

        var accountWithBiggestCode = GetAccountWithBiggestCode(childrenAccount);

        return accountWithBiggestCode;
    }

    private async Task<List<Account>> GetAllParentsLevelsAsync(Account childAccount)
    {
        var allParentsLevels = new List<Account>();

        if (!childAccount.IdParentAccount.HasValue) return allParentsLevels;

        Account? parentAccount;
        do
        {
            parentAccount = await accountRepository.GetByIdAsync(childAccount.IdParentAccount!.Value);

            allParentsLevels.Add(parentAccount!);
            childAccount = parentAccount;
        } while (childAccount.IdParentAccount.HasValue);

        return allParentsLevels;
    }

    private static Account GetAccountWithBiggestCode(IEnumerable<Account> accounts) =>
        accounts.OrderBy(account => account.Code).LastOrDefault()!;

    private static string DefineChildCodeGroup(Account account)
    {
        var accountCode = 1;

        if (account != null) accountCode = account.Code + 1;

        return (account?.Code == 999) ? string.Empty : accountCode.ToString();
    }
}