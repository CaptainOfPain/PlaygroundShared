using System;
using PlaygroundShared.DomainEvents;

namespace PlaygroundShared.Domain
{
    public abstract class BaseAggregateRoot
    {
        private IDomainEventsManager _domainEventsManager;
        public AggregateId Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime ModifiedDate { get; protected set; }

        protected BaseAggregateRoot(AggregateId id, IDomainEventsManager domainEventsManager)
        {
            SetDependencies(domainEventsManager);
            Id = id;
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = CreatedDate;
        }

        protected void DomainEvent(IDomainEvent @event)
        {
            _domainEventsManager.Publish(@event);
        }

        protected void SetDependencies(IDomainEventsManager domainEventsManager)
        {
            _domainEventsManager = domainEventsManager ?? throw new ArgumentNullException(nameof(domainEventsManager));
        }
    }
}