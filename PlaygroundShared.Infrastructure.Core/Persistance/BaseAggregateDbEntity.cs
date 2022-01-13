namespace PlaygroundShared.Infrastructure.Core.Persistance;

public class BaseAggregateDbEntity : BaseDbEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public override Guid Id { get; set; }
}