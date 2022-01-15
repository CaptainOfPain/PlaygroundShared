using Newtonsoft.Json;
using PlaygroundShared.Configurations;
using PlaygroundShared.IntercontextCommunication.Messages;
using RabbitMQ.Client;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq;

public class BusPublisher : IBusPublisher
{
    private readonly IModel _model;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    public BusPublisher(IModel model, RabbitMqConfiguration rabbitMqConfiguration)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _rabbitMqConfiguration = rabbitMqConfiguration ?? throw new ArgumentNullException(nameof(rabbitMqConfiguration));
    }
        
    public Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
    {
        var messageType = typeof(TMessage);
        var genericType = messageType.GetGenericArguments().FirstOrDefault();
        var path = 
            genericType != null ? $"{genericType?.Name}_Failed" 
                : $"{messageType.Name}";

        return PublishAsync(path, message);
    }

    public Task PublishAsync(IMessage message)
    {
        var messageType = message.GetType();
        var genericType = messageType.GetGenericArguments().FirstOrDefault();
        var path = 
            genericType != null ? $"{genericType?.Name}_Failed" 
                : $"{messageType.Name}";

        return PublishAsync(path, message);
    }

    private async Task PublishAsync(string path, IMessage message)
    {
        _model.ExchangeDeclare(
            path,
            _rabbitMqConfiguration.Exchange.Type.ToLowerInvariant(),
            _rabbitMqConfiguration.Exchange.Durable,
            _rabbitMqConfiguration.Exchange.AutoDelete);

        var messageText = JsonConvert.SerializeObject(message);
        byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(messageText);

        var props = _model.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _model.BasicPublish(path, path, props, messageBodyBytes);
        await Task.Yield();
    }
}