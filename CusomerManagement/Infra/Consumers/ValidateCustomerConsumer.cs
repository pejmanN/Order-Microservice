using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class ValidateCustomerConsumer : IConsumer<ValidateCustomer>
    {
        //NOTE :Business Login should move to APPLICATION layer 
        private readonly ICustomerService _customerService;

        public ValidateCustomerConsumer(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task Consume(ConsumeContext<ValidateCustomer> context)
        {
            //NOTE :Business Login should move to APPLICATION layer(like Order Service)

            var isValid = _customerService.CusotmerIsValid(context.Message.CustomerId);


            await context.Publish<CustomerValidated>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });

        }
    }
}
