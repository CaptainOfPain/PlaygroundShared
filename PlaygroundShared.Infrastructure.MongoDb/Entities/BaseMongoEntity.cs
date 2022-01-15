using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PlaygroundShared.Infrastructure.Core.Persistance;

namespace PlaygroundShared.Infrastructure.MongoDb.Entities;

public class BaseMongoEntity : IDbEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
}