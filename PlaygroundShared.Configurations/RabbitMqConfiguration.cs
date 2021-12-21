namespace PlaygroundShared.Configurations;

public class RabbitMqConfiguration
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public int Port { get; set; }
    public List<string> Hostnames { get; set; } = new();
    public RabbitMqExchangeConfig Exchange { get; set; }
    public string QueueNameSuffix { get; set; }
}

public class RabbitMqExchangeConfig
{
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public string Type { get; set; }
}