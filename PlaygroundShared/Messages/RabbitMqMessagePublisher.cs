using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlaygroundShared.RabbitMq;

namespace PlaygroundShared.Messages
{
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly IBusPublisher _busPublisher;

        public RabbitMqMessagePublisher(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        }
        
        public async Task Publish(IMessage message)
        {
            await _busPublisher.PublishAsync(message);
        }
    }
}