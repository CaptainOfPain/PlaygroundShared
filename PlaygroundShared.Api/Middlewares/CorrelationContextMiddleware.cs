using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PlaygroundShared.Domain.Domain;
using PlaygroundShared.Domain.Shared;

namespace PlaygroundShared.Api.Middlewares;

public class CorrelationContextMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationContextMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationContext correlationContext)
    {
        correlationContext.GenerateCorrelationId();
        var userId = context.User.FindFirst("id")?.Value;
        if (userId != null)
        {
            var currentUser = new CurrentUser(new AggregateId(userId), context.User.FindFirst(ClaimTypes.Name).Value);
            correlationContext.SetCurrentUser(currentUser);
        }
            
        await _next(context);
    }
}