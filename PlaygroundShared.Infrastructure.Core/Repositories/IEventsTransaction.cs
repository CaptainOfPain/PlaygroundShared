namespace PlaygroundShared.Infrastructure.Core.Repositories;

public interface IEventsTransaction
{
    void AssignSaveAction(Action action);
    void Execute();
}