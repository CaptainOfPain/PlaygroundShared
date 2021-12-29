namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public class Pipeline<T>
{
    private IFilter<T> root;

    public Pipeline<T> Register(IFilter<T> filter)
    {
        if (root == null)
        {
            root = filter;
        }
        else
        {
            root.Register(filter);
        }

        return this;
    }

    public Task Execute(T context)
    {
        return root.Execute(context);
    }
}