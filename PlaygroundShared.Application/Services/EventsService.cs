using PlaygroundShared.Domain.DomainEvents;
using PlaygroundShared.Messages;

namespace PlaygroundShared.Application.Services
{
    public class EventsService : IEventsService
    {
        private readonly IDomainEventsManager _domainEventsManager;
        private readonly IMessagePublisher _messagePublisher;

        public EventsService(IDomainEventsManager domainEventsManager, IMessagePublisher messagePublisher)
        {
            _domainEventsManager = domainEventsManager ?? throw new ArgumentNullException(nameof(domainEventsManager));
            _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));
        }
        
        public async Task ExecuteEventsAsync()
        {
            var tasksList = _domainEventsManager.DomainEvents.Select(x => _messagePublisher.Publish(x));

            await Task.WhenAll(tasksList);
        }

        public void Clear()
        {
            _domainEventsManager.Clear();
        }
    }
}