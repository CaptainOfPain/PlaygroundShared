using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.Core.Repositories;
using PlaygroundShared.Infrastructure.MongoDb.Repositories;

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
        var mongoConfig = _configuration.GetValue<MongoDbConfiguration>("mongoDbConfig");
        builder.Register(ctx => mongoConfig).SingleInstance();
        
        var mongoClient = new MongoClient(mongoConfig.ConnectionString);
        builder.Register(ctx => mongoClient).As<IMongoClient>().SingleInstance();
        builder.Register(ctx =>
        {
            var mongoDbConfiguration = ctx.Resolve<MongoDbConfiguration>();
            var client = ctx.Resolve<IMongoClient>();
            var database = client.GetDatabase(mongoDbConfiguration.MainDatabaseName);
            return database;
        }).As<IMainMongoDatabase>().InstancePerLifetimeScope();
        
        builder.Register(ctx =>
        {
            var mongoDbConfiguration = ctx.Resolve<MongoDbConfiguration>();
            var client = ctx.Resolve<IMongoClient>();
            var database = client.GetDatabase(mongoDbConfiguration.EventDatabaseName);
            return database;
        }).As<IEventMongoDatabase>().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(_assemblies).Where(x => typeof(IGenericRepository<>).IsAssignableFrom(x))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(_assemblies).Where(x => typeof(IGenericEventRepository<>).IsAssignableFrom(x))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}