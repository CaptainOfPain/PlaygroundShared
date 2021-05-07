using System;
using System.Threading.Tasks;

namespace PlaygroundShared.Application.CQRS
{
    public class CommandQueryDispatcherDecorator : ICommandQueryDispatcherDecorator
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public CommandQueryDispatcherDecorator(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
        }
        
        public Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand => _commandDispatcher.DispatchAsync(command);

        public Task<TDto> DispatchAsync<TQuery, TDto>(TQuery query) where TQuery : IQuery => _queryDispatcher.DispatchAsync<TQuery, TDto>(query);
    }
}