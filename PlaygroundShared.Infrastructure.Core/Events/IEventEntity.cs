namespace PlaygroundShared.Infrastructure.Core.Events;

public interface IEventEntity
{
    public Guid Id { get; }
    public Guid AggregateId { get; }
    public string EventType { get; }
    public DateTime CreatedAt { get; }
    public DateTime? PublishedAt { get; }
    public string Event { get; }
    public string ExecutedBy { get; }
    public Guid CorrelationId { get; }
}