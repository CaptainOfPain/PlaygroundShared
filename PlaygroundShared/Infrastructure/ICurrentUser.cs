using System;
using PlaygroundShared.Domain;

namespace PlaygroundShared
{
    public interface ICurrentUser
    {
        AggregateId? UserId { get; }
        string UserName { get; }
    }
}