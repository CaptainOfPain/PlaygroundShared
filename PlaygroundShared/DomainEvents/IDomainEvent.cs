using System;
using PlaygroundShared.Domain;
using PlaygroundShared.Messages;

namespace PlaygroundShared.DomainEvents
{
    public interface IDomainEvent : IMessage
    {
        public AggregateId Id { get; }
    }
}