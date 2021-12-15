using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using PlaygroundShared.IntercontextCommunication.Messages;

namespace PlaygroundShared.Messages
{
    public interface IMessagePublisher
    {
        Task Publish(IMessage message);
    }
}