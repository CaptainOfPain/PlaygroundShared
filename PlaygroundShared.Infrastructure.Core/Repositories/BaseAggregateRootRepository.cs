using AutoMapper;
using PlaygroundShared.Domain.Domain;
using PlaygroundShared.Domain.DomainEvents;
using PlaygroundShared.Infrastructure.Core.Events;
using PlaygroundShared.Infrastructure.Core.Persistance;

namespace PlaygroundShared.Infrastructure.Core.Repositories;

public abstract class BaseAggregateRootRepository<TAggregate, TEntity, TEventEntity> :
    IAggregateRepository<TAggregate>
    where TAggregate : BaseAggregateRoot 
    where TEntity : IDbEntity 
    where TEventEntity : IEventEntity
{
    protected readonly IGenericRepository<TEntity> Repository;
    protected readonly IGenericEventRepository<TEventEntity> EventRepository;
    protected readonly IDomainEventsManager DomainEventsManager;
    protected readonly IMapper Mapper;
    protected readonly IAggregateRecreate<TAggregate> AggregateRecreate;

    protected BaseAggregateRootRepository(
        IGenericRepository<TEntity> repository,
        IGenericEventRepository<TEventEntity> eventRepository,
        IDomainEventsManager domainEventsManager,
        IMapper mapper,
        IAggregateRecreate<TAggregate> aggregateRecreate)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        EventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        DomainEventsManager = domainEventsManager ?? throw new ArgumentNullException(nameof(domainEventsManager));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        AggregateRecreate = aggregateRecreate ?? throw new ArgumentNullException(nameof(aggregateRecreate));
    }
        
    public virtual async Task PersistAsync(TAggregate aggregate)
    {
        var mappedEntity = MapToEntity(aggregate);
        if (await Repository.ExistsAsync(x => x.Id == aggregate.Id.ToGuid()))
        {
            await Repository.UpdateAsync(mappedEntity);
        }
        else
        {
            await Repository.AddAsync(mappedEntity);
        }

        await SaveEventsAsync(aggregate);
    }

    private async Task SaveEventsAsync(TAggregate aggregate)
    {
        foreach (var domainEvent in DomainEventsManager.DomainEvents.Where(x => x.Id == aggregate.Id))
        {
            await EventRepository.AddAsync(Mapper.Map<TEventEntity>(domainEvent));
        }
    }

    public virtual async Task DeleteAsync(TAggregate aggregate)
    {
        var mappedEntity = MapToEntity(aggregate);
        await Repository.DeleteAsync(mappedEntity);
    }

    public virtual async Task<TAggregate> GetAsync(AggregateId id) => MapToAggregate(await Repository.GetAsync(id.ToGuid()));

    protected virtual TEntity MapToEntity(TAggregate aggregate) => Mapper.Map<TEntity>(aggregate);

    protected virtual TAggregate MapToAggregate(TEntity entity)
    {
        var aggregate = Mapper.Map<TAggregate>(entity);

        if (aggregate != null)
        {
            AggregateRecreate.Init(aggregate);
        }

        return aggregate;
    } 
}