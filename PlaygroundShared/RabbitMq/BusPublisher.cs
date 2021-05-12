using System;
using System.Threading.Tasks;
using PlaygroundShared.Messages;
using RawRabbit;

namespace PlaygroundShared.RabbitMq
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public BusPublisher(IBusClient busClient)
        {
            _busClient = busClient ?? throw new ArgumentNullException(nameof(busClient));
        }
        
        public async Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            var path = $"{messageType.Namespace}.{messageType.Name}";
            
            await _busClient.PublishAsync(message, ctx => ctx.UsePublishConfiguration(cfg =>
            {
                cfg.OnDeclaredExchange(x => x.WithName(path));
                cfg.WithRoutingKey(path);
            }));
        }
    }
}