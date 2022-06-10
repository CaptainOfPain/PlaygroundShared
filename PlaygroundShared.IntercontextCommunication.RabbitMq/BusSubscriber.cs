using Autofac;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.Core.Pipelines;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq.Filters;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq;

public class BusSubscriber : IBusSubscriber
{
    private readonly ISubscriberServiceLocator _subscriberServiceLocator;
    private readonly IModel _model;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    private List<IFilter<BasicDeliverEventArgs>> _filters = new();

    public BusSubscriber(
        ISubscriberServiceLocator subscriberServiceLocator,
        IModel model,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        _subscriberServiceLocator = subscriberServiceLocator ?? throw new ArgumentNullException(nameof(subscriberServiceLocator));
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _rabbitMqConfiguration = rabbitMqConfiguration ?? throw new ArgumentNullException(nameof(rabbitMqConfiguration));
    }

    public IBusSubscriber AddFilter(IFilter<BasicDeliverEventArgs> filter)
    {
        _filters.Add(filter);
        return this;
    }

    public IBusSubscriber SubscribeMessage<TMessage>() where TMessage : IMessage
    {
        var queueName = BindQueueAndExchange(typeof(TMessage));
        var pipeline = PreparePipeline<TMessage>();
        
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += async (ch, ea) =>
        {
            await pipeline.Execute(ea);
            
            _model.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };
        _model.BasicConsume(queueName, false, consumer);
        
        return this;
    }

    private IFilter<BasicDeliverEventArgs> PreparePipeline<TMessage>() where TMessage : IMessage
    {
        var pipelineBuilder = new PipelineBuilder<BasicDeliverEventArgs>(_subscriberServiceLocator);
        pipelineBuilder
            .Register<MessageHandlerFilter<TMessage>>()
            .Register<ExceptionPublisherFilter<TMessage>>();

        foreach (var filter in _filters)
        {
            pipelineBuilder.Register(filter);
        }

        var pipeline = pipelineBuilder.Build();
        return pipeline;
    }

    private string BindQueueAndExchange(Type messageType)
    {
        var genericType = messageType.GetGenericArguments().FirstOrDefault();
        var path = 
            genericType != null ? $"{genericType?.Name}_Failed" 
                : $"{messageType.Name}";
        var queueName = $"{path}_{_rabbitMqConfiguration.QueueNameSuffix}";
        
        _model.ExchangeDeclare(
            path,
            _rabbitMqConfiguration.Exchange.Type.ToLowerInvariant(),
            _rabbitMqConfiguration.Exchange.Durable,
            _rabbitMqConfiguration.Exchange.AutoDelete);
        _model.QueueDeclare(queueName, _rabbitMqConfiguration.Exchange.Durable, false, false, null);
        _model.QueueBind(queueName, path, path, null);

        return queueName;
    }
}
