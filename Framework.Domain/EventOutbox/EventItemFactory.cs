using Framework.Core.Events;
using Newtonsoft.Json;

namespace Framework.Domain.EventOutbox
{
    public static class EventItemFactory
    {
        public static List<DomainEventItem> CreateOutboxItemsFromAggregateRoots(List<IAggregateRoot> aggregateRoots)
        {
            return aggregateRoots.SelectMany(a => a.GetUncommittedEvents())
                .Select(MapToEventItem)
                .ToList();
        }

        private static DomainEventItem MapToEventItem(DomainEvent @event)
        {
            var res = new DomainEventItem()
            {
                EventId = @event.EventId,
                PublishDateTime = @event.PublishDateTime,
                Type = @event.GetType().FullName + ", " + @event.GetType().Assembly.GetName().Name,
                Body = JsonConvert.SerializeObject(@event),
                IsSent = false,
            };

            return res;
        }
    }
}
