using Autofac;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq;

public class AutofacSubscriberServiceLocator : ISubscriberServiceLocator
{
    private readonly IComponentContext _componentContext;

    public AutofacSubscriberServiceLocator(IComponentContext componentContext)
    {
        _componentContext = componentContext;
    }

    public T GetService<T>()
        => _componentContext.Resolve<T>();
}