using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.Messages;

namespace PlaygroundShared.IntercontextCommunication;

public interface IBusPublisher
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    Task PublishAsync(IMessage message);
}