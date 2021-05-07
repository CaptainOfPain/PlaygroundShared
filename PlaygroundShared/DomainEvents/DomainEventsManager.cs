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

        public async Task ExecuteAsync()
        {
            var tasksList = _domainEvents.Select(domainEvent => _messagePublisher.Publish(domainEvent)).ToList();

            await Task.WhenAll(tasksList);
            Clear();
        }

        public void Clear()
        {
            _domainEvents.Clear();
        }
    }
}