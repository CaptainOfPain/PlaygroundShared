namespace PlaygroundShared.Configurations;

public interface IMongoDbConfiguration
{
    public string ConnectionString { get; }
    public string MainDatabaseName { get; }
    public string EventDatabaseName { get; }
}