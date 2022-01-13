namespace PlaygroundShared.Infrastructure.Core.Persistance;

public abstract class BaseDbEntity
{
    public abstract Guid Id { get; set; }
}