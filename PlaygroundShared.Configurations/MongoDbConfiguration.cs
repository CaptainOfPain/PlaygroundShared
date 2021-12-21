namespace PlaygroundShared.Configurations;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; }
    public string MainDatabaseName { get; set; }
    public string EventDatabaseName { get; set; }
}