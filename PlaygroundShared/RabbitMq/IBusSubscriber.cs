using PlaygroundShared.Messages;

namespace PlaygroundShared.RabbitMq
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeMessage<TMessage>() where TMessage : IMessage;
    }
}