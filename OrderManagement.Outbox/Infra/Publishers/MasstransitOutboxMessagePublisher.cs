using Framework.Domain.EventOutbox;
using MassTransit;
using OrderManagement.Outbox.Infra.Persistence;

namespace OrderManagement.Outbox.Infra.Publishers
{
    public class MasstransitOutboxMessagePublisher : IOutboxMessagePublisher
    {
        private readonly IPublishEndpoint _publisher;
        public MasstransitOutboxMessagePublisher(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishAsync(object @event)
        {
            await _publisher.Publish(@event);
        }
    }
}
