using System.Reflection;
using Autofac;
using PlaygroundShared.Domain.Domain;
using PlaygroundShared.Infrastructure.Core.Repositories;
using Module = Autofac.Module;

namespace PlaygroundShared.Infrastructure.Core.IoC;

public class InfrastructureModule : Module
{
    private readonly Assembly[] _assemblies;

    public InfrastructureModule(Assembly[] assemblies)
    {
        _assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterAssemblyTypes(_assemblies).AsClosedTypesOf(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(_assemblies).AsClosedTypesOf(typeof(IGenericEventRepository<>))
            .InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.IsAssignableTo<IRepository>()).AsImplementedInterfaces();
    }
}