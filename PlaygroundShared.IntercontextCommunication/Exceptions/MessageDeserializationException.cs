namespace PlaygroundShared.IntercontextCommunication.Exceptions;

public class MessageDeserializationException : Exception
{
    public MessageDeserializationException(string messageJson) : base($"Cannot deserialize message: \n{messageJson}")
    {
    }
}