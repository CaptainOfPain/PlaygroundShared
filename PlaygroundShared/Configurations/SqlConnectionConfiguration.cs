namespace PlaygroundShared.Configurations
{
    public class SqlConnectionConfiguration : ISqlConnectionConfiguration
    {
        public string MainConnectionString { get; set; }
        public string EventConnectionString { get; set; }
    }
}