﻿using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class CreditCustomerConsumer : IConsumer<CreditCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderService _orderService;

        public CreditCustomerConsumer(ICustomerRepository customerRepository,
                                    IOrderService orderService)
        {
            _customerRepository = customerRepository;
            _orderService = orderService;
        }

        
        public async Task Consume(ConsumeContext<CreditCustomer> context)
        {
            //NOTE :Business Login should move to APPLICATION layer (like Order Service) and Event published using outbox pattern

            //var order = _orderService.GetOrder(context.Message.OrderId);
            //var customer = _customerRepository.Get(context.Message.CustomerId);
            //customer.CreditAccount(order.TotalCost, order.OrderId);


            await context.Publish<CustomerCredited>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });
        }
    }
}
