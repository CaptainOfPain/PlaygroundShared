using System.Linq.Expressions;
using PlaygroundShared.Infrastructure.Core.Persistance;

namespace PlaygroundShared.Infrastructure.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseDbEntity
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> BrowseAsync(Expression<Func<TEntity, bool>> expression = null);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
}