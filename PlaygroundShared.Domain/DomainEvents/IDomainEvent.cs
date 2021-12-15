using PlaygroundShared.Domain.Domain;
using PlaygroundShared.IntercontextCommunication.Messages;

namespace PlaygroundShared.Domain.DomainEvents;

public interface IDomainEvent : IMessage
{
    public AggregateId Id { get; }
}