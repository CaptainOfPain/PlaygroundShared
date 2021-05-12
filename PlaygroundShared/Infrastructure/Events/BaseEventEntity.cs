using System;

namespace PlaygroundShared.Infrastructure.Events
{
    public abstract class BaseEventEntity
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public string EventType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Event { get; set; }
        public string ExecutedBy { get; set; }
        public Guid CorrelationId { get; set; }
    }
}