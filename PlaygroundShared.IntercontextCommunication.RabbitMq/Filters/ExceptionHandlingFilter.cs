using PlaygroundShared.Infrastructure.Core.Pipelines;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Extensions;
using RabbitMQ.Client.Events;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq.Filters;

public class ExceptionPublisherFilter<TMessage> : Filter<BasicDeliverEventArgs> where TMessage : IMessage
{
    private readonly IBusPublisher _busPublisher;

    public ExceptionPublisherFilter(IBusPublisher busPublisher)
    {
        _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
    }
    
    protected override async Task Execute(BasicDeliverEventArgs context, Func<BasicDeliverEventArgs, Task> next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(Exception ex)
        {
            var message = context.DeserializeMessage<TMessage>();
            await _busPublisher.PublishAsync(
                new FailedMessage<TMessage>(message, message.CorrelationId, DateTime.UtcNow, null, ex.Message));

            throw;
        }
    }
}