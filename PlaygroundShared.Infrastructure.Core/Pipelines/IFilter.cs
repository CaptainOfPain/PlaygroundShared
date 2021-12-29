namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public interface IFilter
{
    
}
public interface IFilter<TContext> : IFilter
{
    void Register(IFilter<TContext> filter);
    Task Execute(TContext context);
}