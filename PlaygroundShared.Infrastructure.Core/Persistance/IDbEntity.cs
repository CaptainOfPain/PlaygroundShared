namespace PlaygroundShared.Infrastructure.Core.Persistance;

public interface IDbEntity
{
    public Guid Id { get; set; }
}