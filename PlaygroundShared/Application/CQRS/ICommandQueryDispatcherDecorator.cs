using System.Threading.Tasks;

namespace PlaygroundShared.Application.CQRS
{
    public interface ICommandQueryDispatcherDecorator
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TDto> DispatchAsync<TQuery, TDto>(TQuery command) where TQuery : IQuery;
    }
}