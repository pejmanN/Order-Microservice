using MassTransit;
using Shared.StateMachines.Order.Models;

namespace Inventory.Infra.Consumers
{
    public class AllocateInventoryConsumer : IConsumer<AllocateInventory>
    {
        private readonly ILogger<AllocateInventoryConsumer> _logger;

        public AllocateInventoryConsumer(ILogger<AllocateInventoryConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AllocateInventory> context)
        {
            //allocate inventory business
            _logger.LogInformation("AllocateInventory for order {context.Message.OrderId} and customer {context.Message.CustomerId}",
                context.Message.OrderId, context.Message.CustomerId);
            await context.Publish<InventorAllocated>(new
            {
                context.Message.CustomerId,
                context.Message.OrderId,
            });
        }
    }
}
