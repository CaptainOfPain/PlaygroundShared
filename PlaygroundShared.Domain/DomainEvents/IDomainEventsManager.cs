namespace PlaygroundShared.Domain.DomainEvents;

public interface IDomainEventsManager
{
    IEnumerable<IDomainEvent> DomainEvents { get; }
    void Publish(IDomainEvent domainEvent);
    void Clear();
}