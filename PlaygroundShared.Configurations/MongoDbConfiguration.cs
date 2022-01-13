namespace PlaygroundShared.Configurations;

public class MongoDbConfiguration : IMongoDbConfiguration
{
    public string ConnectionString { get; set; }
    public string MainDatabaseName { get; set; }
    public string EventDatabaseName { get; set; }
}