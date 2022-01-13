using Autofac;
using PlaygroundShared.Application.Services;

namespace PlaygroundShared.Application.CQRS;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IComponentContext _context;
    private readonly IEventsService _eventsService;

    public CommandDispatcher(IComponentContext context, IEventsService eventsService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _eventsService = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
    }
        
    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
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

        await handler.HandleAsync(command);

        await _eventsService.ExecuteEventsAsync();
    }
}