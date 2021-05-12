namespace PlaygroundShared.Configurations
{
    public interface ISqlConnectionConfiguration
    {
        string MainConnectionString { get; }
        string EventConnectionString { get; }
    }
}