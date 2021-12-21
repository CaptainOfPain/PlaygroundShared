namespace PlaygroundShared.IntercontextCommunication.Messages;

public interface IMessage
{
        public Guid CorrelationId { get; }
}