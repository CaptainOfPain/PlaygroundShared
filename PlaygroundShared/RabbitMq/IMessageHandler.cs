using System.Threading;
using System.Threading.Tasks;
using PlaygroundShared.Messages;

namespace PlaygroundShared.RabbitMq
{
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        Task HandleMessageAsync(TMessage message, CancellationToken cancellationToken);
    }
}