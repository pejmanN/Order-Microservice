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
            _logger.LogInformation("call form AcceptOrderActivity");

            var consumeContext = context.GetPayload<ConsumeContext>();

            //_bus.pu
            var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("queue:fulfill-order"));

            //await sendEndpoint.Send<IFulfillOrder>(new
            //{
            //    context.Message.OrderId,
            //    context.Message.CorrelationId,
            //    context.Saga.CustomerId,
            //    PaymentCardNumber = "5099"
            //});

            await next.Execute(context).ConfigureAwait(false);
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
