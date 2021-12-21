using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PlaygroundShared.IntercontextCommunication.RabbitMq;

public static class RawRabbitExtensions
{
    public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
        => app.ApplicationServices.GetService<IBusSubscriber>() ?? throw new ArgumentNullException(nameof(IBusSubscriber));
}