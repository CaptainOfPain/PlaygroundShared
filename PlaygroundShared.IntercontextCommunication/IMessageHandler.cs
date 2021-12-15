using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.Messages;

namespace PlaygroundShared.IntercontextCommunication;

public interface IMessageHandler<in TMessage> where TMessage : IMessage
{
    Task HandleMessageAsync(TMessage message, CancellationToken cancellationToken);
}