using System.Reflection;
using MongoDB.Driver;
using PlaygroundShared.Infrastructure.Core.Repositories;
using PlaygroundShared.Infrastructure.MongoDb.Attribute;
using PlaygroundShared.Infrastructure.MongoDb.Entities;

namespace PlaygroundShared.Infrastructure.MongoDb.Repositories;

public class GenericMongoEventRepository<TEventEntity> : IGenericEventRepository<TEventEntity> where TEventEntity : BaseMongoEventEntity
{
    private readonly IEventMongoDatabase _eventMongoDatabase;
    protected virtual IMongoCollection<TEventEntity> Collection => _eventMongoDatabase.GetCollection<TEventEntity>(typeof(TEventEntity).GetCustomAttribute<MongoCollectionAttribute>().CollectionName);

    public GenericMongoEventRepository(IEventMongoDatabase eventMongoDatabase)
    {
        _eventMongoDatabase = eventMongoDatabase ?? throw new ArgumentNullException(nameof(eventMongoDatabase));
    }
    public async Task AddAsync(TEventEntity eventEntity)
    {
        await Collection.InsertOneAsync(eventEntity);
    }

    public async Task DeleteAsync(TEventEntity eventEntity)
    {
        await Collection.DeleteOneAsync(x => x.Id == eventEntity.Id);
    }

    public async Task<IEnumerable<TEventEntity>> GetForAggregateAsync(Guid aggregateId)
        => (await Collection.FindAsync(x => x.AggregateId == aggregateId)).Current;

    public async Task<TEventEntity> GetAsync(Guid id)
        => (await Collection.FindAsync(x => x.Id == id)).SingleOrDefault();

    public async Task<IEnumerable<TEventEntity>> GetByCorrelationId(Guid correlationId)
        => (await Collection.FindAsync(x => x.CorrelationId == correlationId)).Current;

    public Task SaveAsync()
        => Task.CompletedTask;
}