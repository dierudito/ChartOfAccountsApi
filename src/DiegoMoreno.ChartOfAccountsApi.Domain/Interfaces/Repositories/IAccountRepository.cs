using DiegoMoreno.ChartOfAccountsApi.Domain.Dtos;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories.Base;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
public interface IAccountRepository : IRepositoryWrite<Account>, IRepositoryRead<Account>
{
    Task<Account?> GetAccountByParentAndCodeAsync(Guid? idParentAccount, int code);
    Task<IList<Account>> GetAccountChildrenAsync(Guid id);
    Task<(List<Account> items, int qtd)> SearchAccountAsync(string searchTerm, PaginationDto paginationDto);
}