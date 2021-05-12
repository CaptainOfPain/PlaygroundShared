using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaygroundShared.Infrastructure.Repositories
{
    public interface IEventsTransaction
    {
        void AssignSaveAction(Action action);
        void Execute();
    }

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
}