using MassTransit;
using OrderManagement.Facade;
using Shared.StateMachines.Order.Models;

namespace OrderManagement.Infra.Consumers
{
    public class OrderStatusUpdatedConsumer : IConsumer<OrderStatusUpdated>
    {
        private readonly IOrderFacadeService _orderFacadeService;
        public OrderStatusUpdatedConsumer(IOrderFacadeService orderFacadeService)
        {
            _orderFacadeService = orderFacadeService;
        }
        public async Task Consume(ConsumeContext<OrderStatusUpdated> context)
        {
            await _orderFacadeService.SetOrderStatus(new SetOrderStatusCommand
            {
                OrderId = context.Message.OrderId,
                Status = context.Message.Status,
            });
        }
    }
}
