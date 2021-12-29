using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace PlaygroundShared.Infrastructure.Core.Pipelines;

public class PipelineModule : Module
{
    private readonly Assembly[] _assemblies;

    public PipelineModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }
        
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterGeneric(typeof(PipelineBuilder<>)).InstancePerDependency();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => x.IsAssignableTo<IFilter>()).AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}