using Microsoft.AspNetCore.Builder;

namespace PlaygroundShared.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseEventsPublisherMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EventsPublisherMiddleware>();
        }
    }
}