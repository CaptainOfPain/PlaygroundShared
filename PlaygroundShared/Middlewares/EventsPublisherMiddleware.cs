using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PlaygroundShared.DomainEvents;

namespace PlaygroundShared.Middlewares
{
    public class EventsPublisherMiddleware
    {
        private readonly RequestDelegate _next;

        public EventsPublisherMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IDomainEventsManager domainEventManager)
        {
            try
            {
                await _next(context);
                await domainEventManager.ExecuteAsync();
            }
            catch(Exception)
            {
                domainEventManager.Clear();
                throw;
            }
        }
    }
}