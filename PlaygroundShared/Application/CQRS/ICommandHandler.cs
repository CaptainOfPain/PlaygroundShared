using System.Threading.Tasks;

namespace PlaygroundShared.Application.CQRS
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}