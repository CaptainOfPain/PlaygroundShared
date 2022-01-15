namespace PlaygroundShared.Infrastructure.Core.Persistance;

public class BaseAggregateDbEntity : IDbEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Guid Id { get; set; }
}