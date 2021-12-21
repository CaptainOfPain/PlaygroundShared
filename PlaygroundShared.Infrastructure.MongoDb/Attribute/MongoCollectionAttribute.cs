namespace PlaygroundShared.Infrastructure.MongoDb.Attribute;

public class MongoCollectionAttribute : System.Attribute
{
    public string CollectionName { get; }

    public MongoCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}