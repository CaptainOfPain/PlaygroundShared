using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PlaygroundShared.Api.Middlewares;
using PlaygroundShared.Configurations;
using PlaygroundShared.Domain.Domain;
using PlaygroundShared.Domain.DomainEvents;
using PlaygroundShared.Infrastructure.EF.IoC;
using PlaygroundShared.IntercontextCommunication;
using PlaygroundShared.IntercontextCommunication.Messages;
using PlaygroundShared.IntercontextCommunication.RabbitMq;
using PlaygroundShared.IntercontextCommunication.RabbitMq.IoC;

namespace Playground
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt => {
                    var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey= true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                        IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                        ValidateIssuer = false, 
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    }; 
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Playground", Version = "v1"});
            });
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterType<DomainEventsManager>().As<IDomainEventsManager>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes().Where(x => x.IsAssignableTo(typeof(IAggregateRecreate<>))).AsImplementedInterfaces();
            builder.RegisterModule(new RabbitMqModule("./rawrabbit.json"));
            builder.RegisterMainEfDbContext<TestDbContext>(new SqlConnectionConfiguration()
            {
                MainConnectionString = "Server=localhost;Database=test;Trusted_Connection=True;"
            });
            builder.RegisterType<WeatherForecastMessageHandler>().As<IMessageHandler<WeatherForecast>>();
            builder.RegisterType<WeatherForecastFailedMessageHandler>().As<IMessageHandler<FailedMessage<WeatherForecast>>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Playground v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEventsPublisherMiddleware();
            app.UseCorrelationContextMiddleware();
            app.UseRabbitMq()
                .SubscribeMessage<WeatherForecast>()
                .SubscribeMessage<FailedMessage<WeatherForecast>>();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var busPublisher = app.ApplicationServices.GetService<IBusPublisher>();
            busPublisher.PublishAsync(new WeatherForecast()
            {
                Id = AggregateId.Generate(),
                CorrelationId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Summary = "adadada",
                TemperatureC = 12
            }).GetAwaiter().GetResult();

            var dbContext = app.ApplicationServices.GetService<DbContext>();
        }
    }
    
    public class WeatherForecastMessageHandler : IMessageHandler<WeatherForecast>
    {
        public WeatherForecastMessageHandler()
        {
            
        }
        public Task HandleMessageAsync(WeatherForecast message, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
            // throw new Exception();
            return Task.CompletedTask;
        }
    }
    
    public class WeatherForecastFailedMessageHandler : IMessageHandler<FailedMessage<WeatherForecast>>
    {
        public Task HandleMessageAsync(FailedMessage<WeatherForecast> message, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(message));
            return Task.CompletedTask;
        }
    }
}