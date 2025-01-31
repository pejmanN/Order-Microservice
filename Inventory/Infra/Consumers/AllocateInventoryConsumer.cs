using MassTransit;
using Shared.StateMachines.Order.Models;

namespace Inventory.Infra.Consumers
{
    public class AllocateInventoryConsumer : IConsumer<AllocateInventory>
    {
        public async Task Consume(ConsumeContext<AllocateInventory> context)
        {
            //allocate inventory business
            await context.Publish<InventorAllocated>(new
            {
                context.Message.CustomerId,
                context.Message.OrderId,
            });
        }
    }
}
