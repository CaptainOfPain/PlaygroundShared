using Autofac;
using PlaygroundShared.IntercontextCommunication;

namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public class PipelineBuilder<T>
{
    private readonly ISubscriberServiceLocator _subscriberServiceLocator;

    public PipelineBuilder(ISubscriberServiceLocator subscriberServiceLocator)
    {
        _subscriberServiceLocator = subscriberServiceLocator;
    }
    
    private List<Func<IFilter<T>>> filters = new List<Func<IFilter<T>>>();

    public PipelineBuilder<T> Register(Func<IFilter<T>> filter) 
    {
        filters.Add(filter);
        return this;
    }

    public PipelineBuilder<T> Register(IFilter<T> filter) 
    {
        filters.Add(() => filter);
        return this;
    }

    public PipelineBuilder<T> Register<TFilter>() where TFilter : IFilter<T>
    {
        var filter = _subscriberServiceLocator.GetService<TFilter>();
        filters.Add(() => filter);
        return this;
    }

    public IFilter<T> Build() 
    { 
        var root = filters.First().Invoke();

        foreach (var filter in filters.Skip(1)) 
        {
            root.Register(filter.Invoke());
        }
        
        return root;
    }
}