using Microsoft.AspNetCore.Builder;

namespace PlaygroundShared.Api.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseEventsPublisherMiddleware(this IApplicationBuilder builder) 
        => builder.UseMiddleware<EventsPublisherMiddleware>();

    public static IApplicationBuilder UseCorrelationContextMiddleware(this IApplicationBuilder builder) 
        => builder.UseMiddleware<CorrelationContextMiddleware>();
}