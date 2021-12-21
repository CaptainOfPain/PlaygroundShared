using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PlaygroundShared.Configurations;

namespace PlaygroundShared.Infrastructure.MongoDb.IoC;

public class MongoDbModule : Autofac.Module
{
    private readonly IConfiguration _configuration;

    public MongoDbModule(IConfiguration configuration)
    {
        _configuration = configuration;
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
    }
}