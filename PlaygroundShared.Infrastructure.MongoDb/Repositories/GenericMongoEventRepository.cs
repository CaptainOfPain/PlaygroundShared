using System.Reflection;
using MongoDB.Driver;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.Core.Repositories;
using PlaygroundShared.Infrastructure.MongoDb.Attribute;
using PlaygroundShared.Infrastructure.MongoDb.Entities;

namespace PlaygroundShared.Infrastructure.MongoDb.Repositories;

public class GenericMongoEventRepository<TEventEntity> : IGenericEventRepository<TEventEntity> where TEventEntity : BaseMongoEventEntity
{
    private readonly IMongoDatabase _eventMongoDatabase;
    protected virtual IMongoCollection<TEventEntity> Collection => _eventMongoDatabase.GetCollection<TEventEntity>(typeof(TEventEntity).GetCustomAttribute<MongoCollectionAttribute>().CollectionName);

    public GenericMongoEventRepository(IMongoClient mongoClient, IMongoDbConfiguration mongoDbConfiguration)
    {
        _eventMongoDatabase = mongoClient.GetDatabase(mongoDbConfiguration.EventDatabaseName) ?? throw new ArgumentNullException(nameof(IMongoDatabase));
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