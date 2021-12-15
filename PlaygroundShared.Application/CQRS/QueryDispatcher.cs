using Autofac;

namespace PlaygroundShared.Application.CQRS;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IComponentContext _componentContext;

    public QueryDispatcher(IComponentContext componentContext)
    {
        _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
    }
        
    public Task<TDto> DispatchAsync<TQuery, TDto>(TQuery query) where TQuery : IQuery
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var handler = _componentContext.Resolve<IQueryHandler<TQuery, TDto>>();
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        return handler.HandleAsync(query);
    }
}