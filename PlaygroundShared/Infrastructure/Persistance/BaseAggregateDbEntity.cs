using System;

namespace PlaygroundShared.Infrastructure.Persistance
{
    public class BaseAggregateDbEntity : BaseDbEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}