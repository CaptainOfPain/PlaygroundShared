using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlaygroundShared.Configurations;
using PlaygroundShared.Messages;
using RawRabbit;

namespace PlaygroundShared.RabbitMq
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IBusClient _busClient;
        private readonly RabbitMqConfiguration _rawRabbitConfiguration;

        public BusSubscriber(IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices;
            _busClient = _serviceProvider.GetService<IBusClient>();
            _rawRabbitConfiguration = _serviceProvider.GetService<RabbitMqConfiguration>();
        }

        public IBusSubscriber SubscribeMessage<TMessage>() where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            var path = $"{messageType.Namespace}.{messageType.Name}";
            
            var messageHandler = _serviceProvider.GetService<IMessageHandler<TMessage>>();
            _busClient.SubscribeAsync<TMessage>(async (msg) =>
            {
                await messageHandler.HandleMessageAsync(msg, CancellationToken.None);
            }, ctx =>
            {
                ctx.UseSubscribeConfiguration(cfg =>
                {
                    cfg.Consume(x => x.WithRoutingKey(path));
                    cfg.OnDeclaredExchange(x => x.WithName(path));
                    cfg.FromDeclaredQueue(x => x.WithName(path).WithNameSuffix(_rawRabbitConfiguration.QueueNameSuffix));
                });
            });

            return this;
        }
    }
}