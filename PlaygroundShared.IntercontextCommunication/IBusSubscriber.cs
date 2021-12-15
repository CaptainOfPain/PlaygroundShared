using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.Messages;

namespace PlaygroundShared.IntercontextCommunication;

public interface IBusSubscriber
{
    IBusSubscriber SubscribeMessage<TMessage>() where TMessage : IMessage;
}