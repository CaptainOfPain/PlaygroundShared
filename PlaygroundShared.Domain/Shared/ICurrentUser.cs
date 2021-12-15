using PlaygroundShared.Domain.Domain;

namespace PlaygroundShared.Domain.Shared;

public interface ICurrentUser
{
    AggregateId? UserId { get; }
    string UserName { get; }
}