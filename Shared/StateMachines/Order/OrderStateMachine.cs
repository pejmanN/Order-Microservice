﻿using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Contracts;
using Shared.StateMachines.Order.Models;

namespace Shared.StateMachines.Order
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine(ILogger<OrderState> logger)
        {
            InstanceState(x => x.CurrentState, Submitted, Accepted, Canceled, Faulted);

            SetCorrelationIds();

            Initially
                (
                    When(OrderSubmitted)
                        .Then(x =>
                        {
                            x.Saga.OrderId = x.Message.Id;
                            x.Saga.CustomerId = x.Message.CustomerId;
                            x.Saga.UpdatedTime = DateTime.Now;
                        })
                        .TransitionTo(Submitted)
                        .Activity(x => x.OfType<OrderSubmittedActivity>())
                   .Catch<Exception>(ex => ex.
                        Then(x =>
                        {
                            x.Saga.ErrorMessage = x.Exception.Message;
                            x.Saga.UpdatedTime = DateTime.Now;
                        }).TransitionTo(Faulted))
                );


            During(Submitted,
                    When(CustomerValidated).Then(x =>
                    {
                        x.Saga.UpdatedTime = DateTime.Now;
                    })
                    .Activity(x => x.OfType<CustomerValidatedActivity>())
                    .TransitionTo(Accepted),

                    When(ValidateCustomerFaulted).Then(x =>
                    {
                        x.Saga.ErrorMessage = x.Message.Exceptions[0].Message;
                        x.Saga.UpdatedTime = DateTime.Now;
                    }).TransitionTo(Faulted)
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

            //Event(() => SubmitOrderFaulted, x =>
            //{
            //    x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Message.Id);
            //});
            Event(() => ValidateCustomerFaulted, x =>
            {
                x.CorrelateBy<long>(saga => saga.OrderId, context => context.Message.Message.OrderId);
            });
        }

        public State Submitted { get; private set; }  //value in saga=> 3
        public State Accepted { get; private set; }  //value in saga=> 4
        public State Canceled { get; private set; }  //value in saga=> 5

        public State Faulted { get; private set; } //value in saga=> 6

        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<CustomerValidated> CustomerValidated { get; private set; }
        //public Event<Fault<OrderSubmitted>> SubmitOrderFaulted { get; private set; }
        public Event<Fault<ValidateCustomer>> ValidateCustomerFaulted { get; private set; }
    }
}
