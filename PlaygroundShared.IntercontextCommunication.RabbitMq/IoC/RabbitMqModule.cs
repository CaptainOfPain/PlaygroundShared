using Autofac;
using Newtonsoft.Json;
using PlaygroundShared.Configurations;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Filters;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Messages;
using PlaygroundShared.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Logging;

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
        var config = JsonConvert.DeserializeObject<RabbitMqConfiguration>(File.ReadAllText(_configFilePath));
        builder.Register(ctx => config).SingleInstance();
        var connection = new ConnectionFactory()
        {
            UserName = config.Username,
            Password = config.Password,
            Port = config.Port,
            VirtualHost = config.VirtualHost,
            DispatchConsumersAsync = true,
            HostName = config.Hostnames.FirstOrDefault()
        }.CreateConnection();
        builder
            .Register(ctx => connection.CreateModel())
            .SingleInstance();
        builder
            .RegisterType<BusPublisher>()
            .As<IBusPublisher>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<BusSubscriber>()
            .As<IBusSubscriber>()
            .InstancePerLifetimeScope();
        builder
            .RegisterType<RabbitMqMessagePublisher>()
            .As<IMessagePublisher>()
            .InstancePerLifetimeScope();
        builder
            .RegisterGeneric(typeof(MessageHandlerFilter<>))
            .InstancePerDependency();
        builder
            .RegisterGeneric(typeof(ExceptionPublisherFilter<>))
            .InstancePerDependency();
        builder
            .RegisterType<AutofacSubscriberServiceLocator>().As<ISubscriberServiceLocator>();
    }
}