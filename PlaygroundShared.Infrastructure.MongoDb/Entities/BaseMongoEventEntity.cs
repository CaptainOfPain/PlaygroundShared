using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PlaygroundShared.Infrastructure.Core.Events;

namespace PlaygroundShared.Infrastructure.MongoDb.Entities;

public class BaseMongoEventEntity : IEventEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public Guid AggregateId { get; }
    public string EventType { get; }
    public DateTime CreatedAt { get; }
    public DateTime? PublishedAt { get; }
    public string Event { get; }
    public string ExecutedBy { get; }
    public Guid CorrelationId { get; }
}