using MassTransit;
using Shared.StateMachines.Order.Models;

namespace Inventory.Infra.Consumers
{
    public class DeAllocateInventoryConsumer : IConsumer<DeAllocateInventory>
    {
        private readonly ILogger<DeAllocateInventoryConsumer> _logger;

        public DeAllocateInventoryConsumer(ILogger<DeAllocateInventoryConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<DeAllocateInventory> context)
        {
            //deallocate inventory business
            _logger.LogInformation("DeallocateInventory for order {context.Message.OrderId} and customer {context.Message.CustomerId}",
               context.Message.OrderId, context.Message.CustomerId);
            await context.Publish<DeAllocateInventory>(new
            {
                context.Message.CustomerId,
                context.Message.OrderId,
            });
        }
    }
}
