using DiegoMoreno.ChartOfAccountsApi.Domain.Dtos;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities.Base;
using System.Linq.Expressions;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories.Base;

public interface IRepositoryRead<TEntity> where TEntity : Entity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<(IEnumerable<TEntity> items, int qtd)> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        PaginationDto? paginationDto = null,
        params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetUniqueAsync(Expression<Func<TEntity, bool>> predicate);
}