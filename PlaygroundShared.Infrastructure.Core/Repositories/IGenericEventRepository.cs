using PlaygroundShared.Infrastructure.Core.Events;

namespace PlaygroundShared.Infrastructure.Core.Repositories;

public interface IGenericEventRepository<TEventEntity> where TEventEntity : BaseEventEntity
{
    Task AddAsync(TEventEntity eventEntity);
    Task DeleteAsync(TEventEntity eventEntity);
    Task<IEnumerable<TEventEntity>> GetForAggregateAsync(Guid aggregateId);
    Task<TEventEntity> GetAsync(Guid id);
    Task<IEnumerable<TEventEntity>> GetByCorrelationId(Guid correlationId);
    Task SaveAsync();
}