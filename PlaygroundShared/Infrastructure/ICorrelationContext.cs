using System;

namespace PlaygroundShared.Infrastructure
{
    public interface ICorrelationContext
    {
        Guid CorrelationId { get; }
        ICurrentUser CurrentUser { get; }
        void GenerateCorrelationId();
        void SetCurrentUser(ICurrentUser currentUser);
    }
}