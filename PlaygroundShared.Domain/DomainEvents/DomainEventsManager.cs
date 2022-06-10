namespace PlaygroundShared.Domain.DomainEvents;

public class DomainEventsManager : IDomainEventsManager
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents;

    public DomainEventsManager()
    {
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