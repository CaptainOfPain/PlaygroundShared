using System;
using System.IO;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PlaygroundShared.Configurations;
using PlaygroundShared.Infrastructure.EF.EventDbContext;
using PlaygroundShared.Messages;
using PlaygroundShared.RabbitMq;
using RawRabbit.Channel.Abstraction;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.Autofac;
using RawRabbit.Instantiation;
using RawRabbit.Serialization;

namespace PlaygroundShared.IoC
{
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

        public static ContainerBuilder RegisterRabbitMq(this ContainerBuilder builder, string configFilePath)
        {
            var options = new RawRabbitOptions
            {
                ClientConfiguration = JsonConvert.DeserializeObject<RawRabbitConfiguration>(File.ReadAllText(configFilePath)),
                DependencyInjection = ioc => ioc.AddSingleton<ISerializer, RawRabbitSerializer>()
            };

            builder.RegisterType<RabbitMqMessagePublisher>().As<IMessagePublisher>().InstancePerLifetimeScope();
            return builder.RegisterRawRabbit(options);
        }
    }
}