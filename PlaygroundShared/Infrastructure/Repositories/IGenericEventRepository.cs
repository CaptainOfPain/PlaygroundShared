using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlaygroundShared.Infrastructure.Events;

namespace PlaygroundShared.Infrastructure.Repositories
{
    public interface IGenericEventRepository<TEventEntity> where TEventEntity : BaseEventEntity
    {
        Task AddAsync(TEventEntity eventEntity);
        Task DeleteAsync(TEventEntity eventEntity);
        Task<IEnumerable<TEventEntity>> GetForAggregateAsync(Guid aggregateId);
        Task<TEventEntity> GetAsync(Guid id);
        Task<IEnumerable<TEventEntity>> GetByCorrelationId(Guid correlationId);
        Task SaveAsync();
    }
}