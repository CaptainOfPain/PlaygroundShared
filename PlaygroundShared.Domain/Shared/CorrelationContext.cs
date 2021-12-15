namespace PlaygroundShared.Domain.Shared;

public class CorrelationContext : ICorrelationContext
{
    public Guid CorrelationId { get; private set; }
    public ICurrentUser CurrentUser { get; private set; }

    public void GenerateCorrelationId()
    {
        CorrelationId = Guid.NewGuid();
    }

    public void SetCurrentUser(ICurrentUser currentUser)
    {
        CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    }
}