using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlaygroundShared.Messages;

namespace PlaygroundShared.DomainEvents
{
    public class DomainEventsManager : IDomainEventsManager
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public IEnumerable<IDomainEvent> DomainEvents => _domainEvents;

        public DomainEventsManager(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }
        
        public void Publish(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }
            
            _domainEvents.Add(domainEvent);
        }

        public void Clear()
        {
            _domainEvents.Clear();
        }
    }
}