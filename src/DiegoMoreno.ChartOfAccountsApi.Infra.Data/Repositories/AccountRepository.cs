using DiegoMoreno.ChartOfAccountsApi.Domain.Dtos;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Contexts.Entity;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories;
public class AccountRepository(ChartOfAccountsApiDbContext db) : Repository<Account>(db), IAccountRepository
{
    public async Task<Account?> GetAccountByParentAndCodeAsync(Guid? idParentAccount, int code) =>
        await _dbSet.FirstOrDefaultAsync(account => account.IdParentAccount == idParentAccount && account.Code == code);

    public async Task<IList<Account>> GetAccountChildrenAsync(Guid id) =>
        await _dbSet.Where(account => account.IdParentAccount == id).ToListAsync();

    public async Task<(List<Account> items, int qtd)> SearchAccountAsync(string searchTerm, PaginationDto paginationDto)
    {
        var query = _dbSet
        .Include(account => account.AccountType)
        .Where(account => account.Name.Contains(searchTerm) ||
                          account.Code.ToString().Contains(searchTerm) ||
                          account.AccountType.Name.Contains(searchTerm));

        var count = query.AsNoTracking().Count();

        var items = await query.AsNoTracking().Skip(paginationDto.Skip())
        .Take(paginationDto.Take())
        .ToListAsync();

        return (items, count);
    }
}
