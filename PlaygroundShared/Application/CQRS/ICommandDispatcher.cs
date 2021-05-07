using System.Threading.Tasks;

namespace PlaygroundShared.Application.CQRS
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}