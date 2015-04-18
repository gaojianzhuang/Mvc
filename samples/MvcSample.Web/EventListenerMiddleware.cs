
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;

namespace MvcSample.Web
{
    public class EventListenerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EventListener _listener;

        public EventListenerMiddleware(RequestDelegate next)
        {
            _next = next;
            _listener = new GlimpseListener();
        }

        public async Task Invoke(HttpContext context)
        {
            context.SetFeature(new EventStore());
            await _next(context);
        }

        private class EventStore
        {

        }

        private class GlimpseListener : EventListener
        {
            private HttpContextAccessor _accessor = new HttpContextAccessor();

            protected override void OnEventSourceCreated(EventSource eventSource)
            {
                if (eventSource.Name == "Microsoft.AspNet.Mvc")
                {
                    EnableEvents(eventSource, EventLevel.Verbose);
                }
                else
                {
                    DisableEvents(eventSource);
                }
            }

            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                switch (eventData.EventId)
                {
                    case 1:
                        OnActionStarted((string)eventData.Payload[0], (string)eventData.Payload[1]);
                        break;
                    case 2:
                        OnActionFinished((string)eventData.Payload[0], (string)eventData.Payload[1]);
                        break;
                    default:
                        break;
                }
            }

            private void OnActionStarted(string actionId, string actionDisplayName)
            {
                var eventStore = GetEventStore();
                if (eventStore == null)
                {
                    return;
                }
            }

            private void OnActionFinished(string actionId, string actionDisplayName)
            {
                var eventStore = GetEventStore();
                if (eventStore == null)
                {
                    return;
                }
            }

            private EventStore GetEventStore()
            {
                return _accessor.HttpContext?.GetFeature<EventStore>();
            }
        }
    }
}
