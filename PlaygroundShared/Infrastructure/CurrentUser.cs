using PlaygroundShared.Domain;

namespace PlaygroundShared.Infrastructure
{
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
}