using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Contracts;
using Shared.Constants;
using Shared.StateMachines.Order.Models;

namespace Shared.StateMachines.Order
{
    public class CustomerValidatedActivity : IStateMachineActivity<OrderState, CustomerValidated>
    {
        readonly ILogger<CustomerValidatedActivity> _logger;
        public CustomerValidatedActivity(ILogger<CustomerValidatedActivity> logger)
        {
            _logger = logger;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, CustomerValidated> context, IBehavior<OrderState, CustomerValidated> next)
        {

            var consumeContext = context.GetPayload<ConsumeContext>();
            var sendEndpoint = await consumeContext
                .GetSendEndpoint(new Uri("queue:" + BusConstants.AllocateInventory));

            await sendEndpoint.Send<AllocateInventory>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });


            var sendUpdateStatusEndpoint = await consumeContext
               .GetSendEndpoint(new Uri("queue:" + BusConstants.OrderStatusUpdated));

            await sendUpdateStatusEndpoint.Send<OrderStatusUpdated>(new
            {
                OrderId = context.Message.OrderId,
                Status = context.Saga.CurrentState
            });

        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, CustomerValidated, TException> context,
            IBehavior<OrderState, CustomerValidated> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("customer-validation-activity");
        }
    }
}
