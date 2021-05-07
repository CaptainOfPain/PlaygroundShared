using System;
using System.Threading.Tasks;
using Autofac;

namespace PlaygroundShared.Application.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _context.Resolve<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return handler.HandleAsync(command);
        }
    }
}