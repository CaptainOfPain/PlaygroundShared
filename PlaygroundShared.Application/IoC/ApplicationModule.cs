using System.Reflection;
using Autofac;
using PlaygroundShared.Application.CQRS;
using PlaygroundShared.Application.Services;
using Module = Autofac.Module;

namespace PlaygroundShared.Application.IoC;

public class ApplicationModule : Module
{
    private readonly Assembly[] _assemblies;

    public ApplicationModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.IsAssignableTo<IService>()).AsImplementedInterfaces();
        builder.RegisterAssemblyTypes(_assemblies)
            .AsClosedTypesOf(typeof(ICommandHandler<>));
        builder.RegisterAssemblyTypes(_assemblies)
            .AsClosedTypesOf(typeof(IQueryHandler<,>));
        builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
        builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();
        builder.RegisterType<CommandQueryDispatcherDecorator>().As<ICommandQueryDispatcherDecorator>();
        builder.RegisterType<EventsService>().As<IEventsService>();
    }
}