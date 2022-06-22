using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Contracts;
using Shared.Constants;
using Shared.StateMachines.Order.Models;

namespace Shared.StateMachines.Order
{
    public class OrderSubmittedActivity : IStateMachineActivity<OrderState, OrderSubmitted>
    {
        readonly ILogger<OrderSubmittedActivity> _logger;
        public OrderSubmittedActivity(ILogger<OrderSubmittedActivity> logger)
        {
            _logger = logger;
        }
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, OrderSubmitted> context, IBehavior<OrderState, OrderSubmitted> next)
        {
            throw new NotImplementedException();
            _logger.LogInformation("OrderSubmittedActivity is called");

            var consumeContext = context.GetPayload<ConsumeContext>();
            var sendEndpoint = await consumeContext
                .GetSendEndpoint(new Uri("queue:" + BusConstants.ValidateCustomer));

            await sendEndpoint.Send<ValidateCustomer>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.Id,
            });

            _logger.LogInformation("OrderSubmittedActivity End");
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderSubmitted, TException> context, IBehavior<OrderState, OrderSubmitted> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("submit-order");
        }
    }
}
