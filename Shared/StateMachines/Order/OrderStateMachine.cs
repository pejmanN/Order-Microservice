using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Contracts;
using Quartz.Logging;
using Shared.StateMachines.Order.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.StateMachines.Order
{
    //public class OrderSubmiitedActivityTimeout
    //{
    //    public Guid CorrelationId { get; set; }
    //}
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        //public Schedule<OrderState, OrderSubmiitedActivityTimeout> TimeoutExpired { get; private set; }

        public OrderStateMachine(ILogger<OrderState> logger)
        {

            InstanceState(x => x.CurrentState, Accepted, Validated, ItemGranted, Canceled, Faulted, Completed);

            SetCorrelationIds();

            Initially
                (
                    When(OrderSubmitted)
                        .Then(x =>
                        {
                            x.Saga.OrderId = x.Message.Id;
                            x.Saga.CustomerId = x.Message.CustomerId;
                            x.Saga.UpdatedTime = DateTime.Now;
                            x.Saga.CorrelationId = x.Message.CorrelationId;
                        })
                        .TransitionTo(Accepted)
                        .Activity(x => x.OfType<OrderSubmittedActivity>())
                   .Catch<Exception>(ex => ex.
                        Then(x =>
                        {
                            x.Saga.ErrorMessage = x.Exception.Message;
                            x.Saga.UpdatedTime = DateTime.Now;
                        }).TransitionTo(Faulted))
                );

            During(Accepted,
                Ignore(OrderSubmitted),
                When(CustomerValidated).Then(x =>
                {
                    x.Saga.UpdatedTime = DateTime.Now;
                })
                .TransitionTo(Validated)
                .Activity(x => x.OfType<CustomerValidatedActivity>()),

                When(ValidateCustomerFaulted).Then(x =>
                {
                    x.Saga.ErrorMessage = x.Message.Exceptions[0].Message;
                    x.Saga.UpdatedTime = DateTime.Now;
                }).TransitionTo(Faulted)
           );

            During(Validated,
                Ignore(OrderSubmitted),
                Ignore(CustomerValidated),
                When(InventorAllocated).Then(x =>
                {
                    x.Saga.UpdatedTime = DateTime.Now;
                })
                .TransitionTo(ItemGranted)
                .Send(context => new DebitCustomer
                {
                    OrderId = context.Saga.OrderId,
                    CustomerId = context.Saga.CustomerId
                })
                .Send(context => new OrderStatusUpdated
                {
                    OrderId = context.Saga.OrderId,
                    Status = context.Saga.CurrentState
                }),

                When(InventorAllocatedFaulted).Then(x =>
                {
                    x.Saga.UpdatedTime = DateTime.Now;
                    x.Saga.ErrorMessage = x.Message.Exceptions[0].Message;
                })
                .TransitionTo(Faulted)
            );

            During(ItemGranted,
                Ignore(OrderSubmitted),
                Ignore(CustomerValidated),
                Ignore(InventorAllocated),
                When(CustomerDebited).Then(x =>
                {
                    logger.LogInformation("Order process is completed for Customer {context.Message.CustomerId}, Order ={context.Message.OrderId}",
                                         x.Message.CustomerId, x.Message.OrderId);
                    x.Saga.UpdatedTime = DateTime.Now;
                })
                .TransitionTo(Completed)
                .Send(context => new OrderStatusUpdated
                {
                    OrderId = context.Saga.OrderId,
                    Status = context.Saga.CurrentState
                }),

                When(CustomerDebitedFaulted)
                .Then(x =>
                    {
                        x.Saga.ErrorMessage = x.Message.Exceptions[0].Message;
                        x.Saga.UpdatedTime = DateTime.Now;
                    })
                .TransitionTo(Faulted)
                .Send(context => new DeAllocateInventory
                {
                    CustomerId = context.Saga.CustomerId,
                    OrderId = context.Saga.OrderId,
                }));


            WhenEnter(Faulted, eventActivity =>
            {

                return eventActivity.Send(context => new OrderStatusUpdated
                {
                    OrderId = context.Saga.OrderId,
                    Status = context.Saga.CurrentState
                });


            });

            During(Completed, Faulted,
                Ignore(OrderSubmitted),
                Ignore(CustomerValidated),
                Ignore(InventorAllocated),
                Ignore(CustomerDebited)
           );

        }

        private void SetCorrelationIds()
        {
            Event(() => OrderSubmitted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Id);
            });

            Event(() => CustomerValidated, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.OrderId);
            });
            Event(() => ValidateCustomerFaulted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Message.OrderId);
            });

            Event(() => InventorAllocated, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.OrderId);
            });
            Event(() => InventorAllocatedFaulted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Message.OrderId);
            });

            Event(() => CustomerDebited, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.OrderId);
            });
            Event(() => CustomerDebitedFaulted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Message.OrderId);
            });
        }

        public State Accepted { get; private set; }  //value in saga=> 3
        public State Validated { get; private set; }  //value in saga=> 4
        public State ItemGranted { get; set; }   //value in saga=> 5
        public State Canceled { get; private set; }  //value in saga=> 6
        public State Faulted { get; private set; } //value in saga=> 7
        public State Completed { get; private set; } //value in saga=> 8


        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<CustomerValidated> CustomerValidated { get; private set; }
        public Event<Fault<ValidateCustomer>> ValidateCustomerFaulted { get; private set; }
        public Event<InventorAllocated> InventorAllocated { get; private set; }
        public Event<Fault<InventorAllocated>> InventorAllocatedFaulted { get; private set; }
        public Event<CustomerDebited> CustomerDebited { get; private set; }
        public Event<Fault<CustomerDebited>> CustomerDebitedFaulted { get; private set; }
    }
}
