using Autofac;
using Microsoft.EntityFrameworkCore;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.EF.Contexts;

namespace PlaygroundShared.Infrastructure.EF.IoC;

public static class ContainerRegistrationExtensions
{
    public static ContainerBuilder RegisterMainEfDbContext<TDbContext>(this ContainerBuilder builder, ISqlConnectionConfiguration sqlConnectionConfiguration) where TDbContext : DbContext
    {
        builder.Register(ctx =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseSqlServer(sqlConnectionConfiguration.MainConnectionString);

            return (TDbContext) Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
        }).As<DbContext>().InstancePerLifetimeScope();

        return builder;
    }
        
        
    public static ContainerBuilder RegisterEventEfDbContext<TDbContext>(this ContainerBuilder builder, ISqlConnectionConfiguration sqlConnectionConfiguration) where TDbContext : EventDbContext
    {
        builder.Register(ctx =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            optionsBuilder.UseSqlServer(sqlConnectionConfiguration.EventConnectionString);

            return (TDbContext) Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
        }).As<EventDbContext>().InstancePerLifetimeScope();

        return builder;
    }
}