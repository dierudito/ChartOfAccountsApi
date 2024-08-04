using DiegoMoreno.ChartOfAccountsApi.Domain.Entities.Base;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories.Base;
public interface IRepositoryWrite<TEntity> where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<int> CommitAsync();
}
