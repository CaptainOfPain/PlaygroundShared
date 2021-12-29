using Newtonsoft.Json;
using PlaygroundShared.Infrastructure.Core.Pipelines;
using PlaygroundShared.IntercontextCommunication.Exceptions;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Extensions;
using RabbitMQ.Client.Events;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq.Filters;

public class MessageHandlerFilter<TMessage> : Filter<BasicDeliverEventArgs> where TMessage : IMessage
{
    private readonly IMessageHandler<TMessage> _messageHandler;

    public MessageHandlerFilter(IMessageHandler<TMessage> messageHandler)
    {
        _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
    }
    
    protected override async Task Execute(BasicDeliverEventArgs context, Func<BasicDeliverEventArgs, Task> next)
    {
        await _messageHandler.HandleMessageAsync(context.DeserializeMessage<TMessage>(), CancellationToken.None);
    }
}