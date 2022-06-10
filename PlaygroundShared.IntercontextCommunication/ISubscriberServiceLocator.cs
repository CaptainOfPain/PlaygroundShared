namespace PlaygroundShared.IntercontextCommunication;

public interface ISubscriberServiceLocator
{
    T GetService<T>();
}