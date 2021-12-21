using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaygroundShared.Configurations;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq;

public class BusSubscriber : IBusSubscriber
{
    private readonly IComponentContext _componentContext;
    private readonly IModel _model;
    private readonly IBusPublisher _busPublisher;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    public BusSubscriber(IComponentContext componentContext, IModel model, IBusPublisher busPublisher, RabbitMqConfiguration rabbitMqConfiguration)
    {
        _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _busPublisher = busPublisher ?? throw new ArgumentNullException(nameof(busPublisher));
        _rabbitMqConfiguration = rabbitMqConfiguration ?? throw new ArgumentNullException(nameof(rabbitMqConfiguration));
    }

    public IBusSubscriber SubscribeMessage<TMessage>() where TMessage : IMessage
    {
        var messageType = typeof(TMessage);
        var genericType = messageType.GetGenericArguments().FirstOrDefault();
        var path = 
            genericType != null ? $"{genericType?.Name}_Failed" 
                : $"{messageType.Name}";
        var queueName = $"{path}_{_rabbitMqConfiguration.QueueNameSuffix}";
        var messageHandler = _componentContext.Resolve<IMessageHandler<TMessage>>();
        
        _model.ExchangeDeclare(
            path,
            _rabbitMqConfiguration.Exchange.Type.ToLowerInvariant(),
            _rabbitMqConfiguration.Exchange.Durable,
            _rabbitMqConfiguration.Exchange.AutoDelete);
        _model.QueueDeclare(queueName, _rabbitMqConfiguration.Exchange.Durable, false, false, null);
        _model.QueueBind(queueName, path, path, null);
        
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = System.Text.Encoding.UTF8.GetString(body);
            var message = JsonConvert.DeserializeObject<TMessage>(text);

            try
            {
                await messageHandler.HandleMessageAsync(message, new CancellationToken());
            }
            catch (Exception ex)
            {
                await _busPublisher.PublishAsync(
                    new FailedMessage<TMessage>(message, message.CorrelationId, DateTime.UtcNow, null, ex.Message));
            }
            
            _model.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };
        _model.BasicConsume(queueName, false, consumer);
        
        return this;
    }
}
