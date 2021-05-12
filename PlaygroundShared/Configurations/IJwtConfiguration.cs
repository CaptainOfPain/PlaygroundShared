namespace PlaygroundShared.Configurations
{
    public interface IJwtConfiguration
    {
        string Secret { get; }
        int ExpiresHours { get; }
    }
}