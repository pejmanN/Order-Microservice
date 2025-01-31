using Framework.Application;
using Framework.Core.Events;
using OrderManagement.Domain.Contracts;

namespace OrderManagement.Facade
{
    public class OrderFacadeService : IOrderFacadeService
    {
        private readonly ICommandBus _bus;
        private readonly IEventAggregator _listener;
        public OrderFacadeService(ICommandBus bus, IEventAggregator listener)
        {
            _bus = bus;
            this._listener = listener;
        }

        public async Task<Guid> Create(SubmitOrderCommand command)
        {
            Guid correlationId = Guid.Empty;
            _listener.Subscribe(new ActionEventHandler<OrderSubmitted>(orderSubmitted => correlationId = orderSubmitted.CorrelationId));
            await _bus.Dispatch(command);

            return correlationId;
        }
    }
}
