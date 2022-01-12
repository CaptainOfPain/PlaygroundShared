
using System.Reflection;
using Autofac;
using PlaygroundShared.Domain.Domain;
using PlaygroundShared.Domain.DomainEvents;
using Module = Autofac.Module;

namespace PlaygroundShared.Domain.IoC;

public class DomainModule : Module
{
    private readonly Assembly[] _assemblies;

    public DomainModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.RegisterAssemblyTypes(_assemblies).AsClosedTypesOf(typeof(IAggregateRecreate<>)).AsImplementedInterfaces();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.Name.EndsWith("DomainEventFactory") && x.IsClass).AsImplementedInterfaces();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.Name.EndsWith("PolicyFactory") && x.IsClass).AsImplementedInterfaces();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.IsAssignableTo<IDomainFactory>())
            .AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<DomainEventsManager>().As<IDomainEventsManager>().InstancePerLifetimeScope();
    }
}