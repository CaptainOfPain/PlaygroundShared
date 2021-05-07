using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace PlaygroundShared.Messages
{
    public interface IMessagePublisher
    {
        Task Publish(IMessage message);
    }
}