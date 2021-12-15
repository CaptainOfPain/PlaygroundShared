namespace PlaygroundShared.Configurations
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string Secret { get; set; }
        public int ExpiresHours { get; set; }
    }
}