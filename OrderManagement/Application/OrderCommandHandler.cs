using Framework.Application;
using Framework.Core.Events;
using OrderManagement.Domain.Order;
using OrderManagement.Facade;

namespace OrderManagement.Application
{
    public class OrderCommandHandler : ICommandHandler<SubmitOrderCommand>,
                                       ICommandHandler<SetOrderStatusCommand>
    {
        private readonly ILogger<OrderCommandHandler> _logger;
        private readonly IEventAggregator _publisher;
        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(
            ILogger<OrderCommandHandler> logger,
            IEventAggregator publisher,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _publisher = publisher;
            _orderRepository = orderRepository;
        }

        public async Task Handle(SubmitOrderCommand command)
        {
            try
            {
                _logger.LogInformation("OrderCommandHandler for SubmitOrderCommand called,CustomerId: {CustomerId}", command.CustomerId);

                var newOrderId = _orderRepository.GetNextId();
                Guid correlationId = Guid.NewGuid();
                var order = new Order(newOrderId, command.CustomerId, DateTime.Now, ToOrderLines(command.OrderLines), _publisher, correlationId);

                _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();

                _logger.LogInformation("OrderCommandHandler for SubmitOrderCommand end,CustomerId: {CustomerId} , " +
                    "orderId : {orderId}", command.CustomerId, newOrderId);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task Handle(SetOrderStatusCommand command)
        {
            _logger.LogInformation("OrderCommandHandler for SetOrderStatusCommand called,orderId: {orderId}", command.OrderId);
            var order = _orderRepository.Get(command.OrderId);
            if (order is null)
                throw new Exception($"Order with provided Orderid= {command.OrderId} does not exist");

            order.SetStatus(command.Status);

            await _orderRepository.SaveChangesAsync();
        }

        private List<OrderLine> ToOrderLines(List<OrderLineCommand> orderLines)
        {
            var result = new List<OrderLine>();
            foreach (var line in orderLines)
            {
                result.Add(new OrderLine(line.ProductId, line.Quantity, line.EachPrice));
            }

            return result;
        }
    }
}
