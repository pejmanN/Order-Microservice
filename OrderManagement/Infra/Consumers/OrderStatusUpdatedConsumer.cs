using MassTransit;
using OrderManagement.Facade;
using Shared.StateMachines.Order.Models;

namespace OrderManagement.Infra.Consumers
{
    public class OrderStatusUpdatedConsumer : IConsumer<OrderStatusUpdated>
    {
        private readonly IOrderFacadeService _orderFacadeService;

        private readonly ILogger<OrderStatusUpdatedConsumer> _logger;
        public OrderStatusUpdatedConsumer(IOrderFacadeService orderFacadeService, 
            ILogger<OrderStatusUpdatedConsumer> logger)
        {
            _orderFacadeService = orderFacadeService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderStatusUpdated> context)
        {
            _logger.LogInformation("OrderStatusUpdatedConsumer for  OrderId {OrderId}, Status ={Status}",
                                         context.Message.OrderId, context.Message.Status);
            await _orderFacadeService.SetOrderStatus(new SetOrderStatusCommand
            {
                OrderId = context.Message.OrderId,
                Status = context.Message.Status,
            });
        }
    }

    public class OrderStatusUpdatedConsumerDefinition : ConsumerDefinition<OrderStatusUpdatedConsumer>
    {
        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<OrderStatusUpdatedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r =>
            {
                r.None();
            });

            //endpointConfigurator.DiscardFaultedMessages();
        }
    }
}
