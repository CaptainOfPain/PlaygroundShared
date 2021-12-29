using Newtonsoft.Json;
using PlaygroundShared.IntercontextCommunication.Exceptions;
using RabbitMQ.Client.Events;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq.Extensions;

public static class MessageHelper
{
    public static TMessage DeserializeMessage<TMessage>(this BasicDeliverEventArgs basicDeliverEventArgs)
    {
        var body = basicDeliverEventArgs.Body.ToArray();
        var text = System.Text.Encoding.UTF8.GetString(body);
        return JsonConvert.DeserializeObject<TMessage>(text) ?? throw new MessageDeserializationException(text);
    }
}