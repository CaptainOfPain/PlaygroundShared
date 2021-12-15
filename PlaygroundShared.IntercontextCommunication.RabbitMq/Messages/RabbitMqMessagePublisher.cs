using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.Messages;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq.Messages;

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