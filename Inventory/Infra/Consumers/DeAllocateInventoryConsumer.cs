using MassTransit;
using Shared.StateMachines.Order.Models;

namespace Inventory.Infra.Consumers
{
    public class DeAllocateInventoryConsumer : IConsumer<DeAllocateInventory>
    {
        public async Task Consume(ConsumeContext<DeAllocateInventory> context)
        {
            //deallocate inventory business
            await context.Publish<DeAllocateInventory>(new
            {
                context.Message.CustomerId,
                context.Message.OrderId,
            });
        }
    }
}
