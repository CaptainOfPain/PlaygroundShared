using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PlaygroundShared.Infrastructure.Core.Events;

namespace PlaygroundShared.Infrastructure.MongoDb.Entities;

public class BaseMongoEventEntity : BaseEventEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public override Guid Id { get; set; }
}