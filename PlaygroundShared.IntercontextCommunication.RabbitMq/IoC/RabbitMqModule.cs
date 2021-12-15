using Autofac;
using Newtonsoft.Json;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Messages;
using PlaygroundShared.Messages;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.Autofac;
using RawRabbit.Instantiation;
using RawRabbit.Serialization;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq.IoC;

public class RabbitMqModule : Autofac.Module
{
    private readonly string _configFilePath;

    public RabbitMqModule(string configFilePath)
    {
        _configFilePath = configFilePath;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        var options = new RawRabbitOptions
        {
            ClientConfiguration = JsonConvert.DeserializeObject<RawRabbitConfiguration>(File.ReadAllText(_configFilePath)),
            DependencyInjection = ioc => ioc.AddSingleton<ISerializer, RawRabbitSerializer>()
        };

        builder.RegisterType<RabbitMqMessagePublisher>().As<IMessagePublisher>().InstancePerLifetimeScope();
        builder.RegisterRawRabbit(options);
    }
}