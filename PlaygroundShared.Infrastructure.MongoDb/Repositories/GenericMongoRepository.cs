using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Driver;
using PlaygroundShared.Infrastructure.Core.Persistance;
using PlaygroundShared.Infrastructure.Core.Repositories;
using PlaygroundShared.Infrastructure.MongoDb.Attribute;
using PlaygroundShared.Infrastructure.MongoDb.Entities;

namespace PlaygroundShared.Infrastructure.MongoDb.Repositories;

public class GenericMongoRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseMongoEntity
{
    private readonly IMainMongoDatabase _mongoDatabase;

    protected IMongoCollection<TEntity> Collection =>
        _mongoDatabase.GetCollection<TEntity>(typeof(TEntity).GetCustomAttribute<MongoCollectionAttribute>()
            .CollectionName);

    public GenericMongoRepository(IMainMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
    }
    
    public async Task AddAsync(TEntity entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions() {IsUpsert = true});
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Collection.DeleteOneAsync(e => e.Id == entity.Id);
    }

    public async Task<TEntity?> GetAsync(Guid id)
        => (await Collection.FindAsync(x => x.Id == id)).SingleOrDefault();


    public async Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        => (await Collection.FindAsync(expression)).FirstOrDefault();

    public async Task<IEnumerable<TEntity>> BrowseAsync(Expression<Func<TEntity, bool>>? expression = null)
        => expression == null ? await Collection.AsQueryable().ToListAsync() : (await Collection.FindAsync(expression)).Current;

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        => (await GetByExpressionAsync(expression)) != null;
}