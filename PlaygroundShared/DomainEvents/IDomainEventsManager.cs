using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundShared.DomainEvents
{
    public interface IDomainEventsManager
    {
        IEnumerable<IDomainEvent> DomainEvents { get; }
        void Publish(IDomainEvent domainEvent);
        void Clear();
    }
}