using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Contracts;
using Shared.StateMachines.Order.Models;

namespace Shared.StateMachines.Order
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine(ILogger<OrderState> logger)
        {
            InstanceState(x => x.CurrentState, Submitted);

            SetCorrelationIds();


            Initially(
                When(OrderSubmitted)
                    .Then(x =>
                    {
                        x.Saga.OrderId = x.Message.Id;
                        x.Saga.CustomerId = x.Message.CustomerId;
                        x.Saga.UpdatedTime = DateTime.Now;
                    })
                    .TransitionTo(Submitted)
                );
        }

        private void SetCorrelationIds()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Id);
            });
        }

        public State Submitted { get; private set; }  //value in saga=> 3

        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
    }
}
