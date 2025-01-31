using CusomerManagement.Domain.Customer;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class ValidateCustomerConsumer : IConsumer<ValidateCustomer>
    {
        private readonly ICustomerRepository _customerRepository;

        public ValidateCustomerConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Consume(ConsumeContext<ValidateCustomer> context)
        {

            try
            {
                var customer = _customerRepository.Get(context.Message.CustomerId);

                //if (cu)
                //{
                    
                //}
                //validate customer business
                await Task.Delay(500);

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
                throw new Exception($"Customer with  {context.Message.CustomerId} is invalid");
            }
        }
    }
}
