﻿using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class ValidateCustomerConsumer : IConsumer<ValidateCustomer>
    {
        public async Task Consume(ConsumeContext<ValidateCustomer> context)
        {
            try
            {
                //validate customer

                await context.Publish<CustomerValidated>(new
                {
                    CustomerId = context.Message.CustomerId,
                    OrderId = context.Message.OrderId,
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError("error occurd", ex);
                //publish invalid customer event
            }
        }
    }
}
