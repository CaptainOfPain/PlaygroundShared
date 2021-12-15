namespace PlaygroundShared.Infrastructure.Core.Repositories;

public class EventsTransaction : IEventsTransaction
{
    private readonly List<Action> _actions = new List<Action>();

    public void AssignSaveAction(Action action)
    {
        _actions.Add(action);
    }

    public void Execute()
    {
        foreach (var action in _actions)
        {
            action.Invoke();
        }
    }
}