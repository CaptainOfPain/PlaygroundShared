using Autofac;

namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public class PipelineBuilder<T>
{
    private readonly IComponentContext _componentContext;

    public PipelineBuilder(IComponentContext componentContext)
    {
        _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
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
        var filter = _componentContext.Resolve<TFilter>();
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