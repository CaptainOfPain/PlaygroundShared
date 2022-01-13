using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.Core.Repositories;

namespace PlaygroundShared.Infrastructure.MongoDb.IoC;

public class MongoDbModule : Autofac.Module
{
    private readonly IConfiguration _configuration;
    private readonly Assembly[] _assemblies;

    public MongoDbModule(IConfiguration configuration, params Assembly[] assemblies)
    {
        _configuration = configuration;
        _assemblies = assemblies;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        var mongoConfig = new MongoDbConfiguration();
        _configuration.Bind("MongoDbConfig", mongoConfig);
        builder.Register(ctx => mongoConfig).As<IMongoDbConfiguration>().SingleInstance();
        
        var mongoClient = new MongoClient(mongoConfig.ConnectionString);
        builder.Register(ctx => mongoClient).As<IMongoClient>().SingleInstance();

        builder.RegisterAssemblyTypes(_assemblies).Where(x => typeof(IGenericRepository<>).IsAssignableFrom(x))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => typeof(IGenericEventRepository<>).IsAssignableFrom(x))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}