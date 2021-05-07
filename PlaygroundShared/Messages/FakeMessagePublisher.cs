using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlaygroundShared.Messages
{
    public class FakeMessagePublisher : IMessagePublisher
    {
        public Task Publish(IMessage message)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
            
            return Task.CompletedTask;
        }
    }
}