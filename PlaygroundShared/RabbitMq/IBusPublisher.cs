using System.Threading.Tasks;
using PlaygroundShared.Messages;
using RawRabbit.Serialization;

namespace PlaygroundShared.RabbitMq
{
    public interface IBusPublisher
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}