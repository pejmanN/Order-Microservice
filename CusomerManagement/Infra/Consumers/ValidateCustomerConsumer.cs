using CusomerManagement.Domain.Customer;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class ValidateCustomerConsumer : IConsumer<ValidateCustomer>
    {
        //NOTE :Business Login should move to APPLICATION layer 
        private readonly ICustomerRepository _customerRepository;

        public ValidateCustomerConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<ValidateCustomer> context)
        {
            //NOTE :Business Login should move to APPLICATION layer(like Order Service)
            var customer = _customerRepository.Get(context.Message.CustomerId);
            if (customer is null || customer.Disabled)
                throw new Exception("Invalid Customer");

            await context.Publish<CustomerValidated>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });
        }
    }
}
