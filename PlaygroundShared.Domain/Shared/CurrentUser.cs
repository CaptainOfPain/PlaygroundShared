using PlaygroundShared.Domain.Domain;

namespace PlaygroundShared.Domain.Shared;

public class CurrentUser : ICurrentUser
{
    public AggregateId? UserId { get; }
    public string UserName { get; }

    public CurrentUser(AggregateId? userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}