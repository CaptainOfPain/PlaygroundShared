using PlaygroundShared.Infrastructure.Core.Persistance;

namespace PlaygroundShared.Infrastructure.EF;

public class BaseEfEntity : IDbEntity
{
    public Guid Id { get; set; }
}