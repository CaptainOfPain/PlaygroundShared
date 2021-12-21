using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PlaygroundShared.Infrastructure.Core.Persistance;

namespace PlaygroundShared.Infrastructure.MongoDb.Entities;

public class BaseMongoEntity : BaseDbEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public new Guid Id { get; set; }
}