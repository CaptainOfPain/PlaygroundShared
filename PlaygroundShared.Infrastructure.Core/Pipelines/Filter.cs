namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public abstract class Filter<TContext> : IFilter<TContext>
{
    private IFilter<TContext> next;
    protected abstract Task Execute(TContext context, Func<TContext, Task> next);

    public void Register(IFilter<TContext> filter)
    {
        if (next == null)
        {
            next = filter;
        }
        else
        {
            next.Register(filter);
        }
    }

    Task IFilter<TContext>.Execute(TContext context)
    {
        return Execute(context, ctx => next == null 
            ? Task.CompletedTask
            : next.Execute(ctx));
    }
}