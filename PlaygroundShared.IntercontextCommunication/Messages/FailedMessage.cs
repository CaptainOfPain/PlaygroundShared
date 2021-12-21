namespace PlaygroundShared.IntercontextCommunication.Messages;

public class FailedMessage<TMessage> : IMessage where TMessage : IMessage
{
    public Guid Id { get; private set; }
    public TMessage OriginalMessage { get; private set; }
    public Guid CorrelationId { get; private set; }
    public DateTime FailedDate { get; private set; }
    public Guid? UserId { get; private set; }
    public string ExceptionMessage { get; private set; }

    public FailedMessage(TMessage originalMessage, Guid correlationId, DateTime failedDate, Guid? userId,
        string exceptionMessage)
    {
        Id = Guid.NewGuid();
        OriginalMessage = originalMessage;
        CorrelationId = correlationId;
        FailedDate = failedDate;
        UserId = userId;
        ExceptionMessage = exceptionMessage;
    }

    private FailedMessage()
    {
        
    }
}