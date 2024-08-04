using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Contexts.Entity;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories.Base;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories;

public class AccountTypeRepository(ChartOfAccountsApiDbContext db) : Repository<AccountType>(db), IAccountTypeRepository
{
}
