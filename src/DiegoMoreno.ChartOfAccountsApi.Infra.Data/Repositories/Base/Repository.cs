using DiegoMoreno.ChartOfAccountsApi.Domain.Dtos;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities.Base;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories.Base;
using DiegoMoreno.ChartOfAccountsApi.Infra.Data.Contexts.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.Data.Repositories.Base;
public class Repository<TEntity>(ChartOfAccountsApiDbContext db) : IDisposable, IRepositoryWrite<TEntity>, IRepositoryRead<TEntity> where TEntity : Entity, new()
{
    protected DbSet<TEntity> _dbSet = db.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task<int> CommitAsync() => await db.SaveChangesAsync();

    public async Task DeleteAsync(Guid id)
    {
        var entity = new TEntity { Id = id };
        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() => _dbSet.AsNoTracking();

    public async Task<(IEnumerable<TEntity> items, int qtd)> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        PaginationDto? paginationDto = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        await Task.Yield();
        var query = _dbSet.AsQueryable();
        int count = 0;

        if (predicate != null) query = _dbSet.AsQueryable().Where(predicate);

        if (includes != null)
            query = includes.Aggregate(
                query,
                (current, includeProperty) => current.Include(includeProperty));

        count = query.AsNoTracking().Count();
        if (paginationDto != null)
            query = query.Skip(paginationDto.Skip()).Take(paginationDto.Take());

        return (query.AsNoTracking().AsEnumerable(), count);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(reposiotry => reposiotry.Id == id);

    public async Task<TEntity?> GetUniqueAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _dbSet.FirstAsync(predicate);

    public void Dispose()
    {
        db?.Dispose();
        GC.SuppressFinalize(this);
    }
}