using RawRabbit.Configuration;

namespace PlaygroundShared.Configurations
{
    public class RabbitMqConfiguration : RawRabbitConfiguration
    {
        public string QueueNameSuffix { get; set; }
    }
}