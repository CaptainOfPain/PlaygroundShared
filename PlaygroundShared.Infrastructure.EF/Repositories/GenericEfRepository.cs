using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PlaygroundShared.Infrastructure.Core.Repositories;

namespace PlaygroundShared.Infrastructure.EF.Repositories;

public abstract class GenericEfRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEfEntity
{
    private readonly DbContext _context;
    protected DbSet<TEntity> Set => _context.Set<TEntity>();

    public GenericEfRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        await SaveAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        var existingEntity = await GetAsync(entity.Id);
        UpdateExistingEntity(existingEntity, entity);

        Set.Update(existingEntity);
        await SaveAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        Set.Remove(entity);
        await SaveAsync();
    }

    public virtual async Task<TEntity> GetAsync(Guid id)
        => await Set.SingleOrDefaultAsync(x => x.Id == id);

    public virtual async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        => await Set.SingleOrDefaultAsync(expression);

    public virtual async Task<IEnumerable<TEntity>> BrowseAsync(Expression<Func<TEntity, bool>> expression = null)
    {
        if (expression == null)
        {
            return await Set.ToListAsync();
        }

        return await Set.Where(expression).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        => await Set.AnyAsync(expression);

    protected virtual void UpdateExistingEntity(TEntity existingEntity, TEntity entity)
    {
        var entityTypeProperties = entity.GetType().GetProperties();
        var existingTypeProperty = existingEntity.GetType();

        foreach (var property in entityTypeProperties)
        {
            existingTypeProperty.GetProperty(property.Name)?.SetValue(existingEntity, property.GetValue(entity));
        }
    }

    protected virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}