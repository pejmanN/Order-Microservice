using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class CustomerDebitConsumer : IConsumer<DebitCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderService _orderService;

        public CustomerDebitConsumer(ICustomerRepository customerRepository,
                                    IOrderService orderService)
        {
            _customerRepository = customerRepository;
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<DebitCustomer> context)
        {
            //NOTE :Business Login should move to APPLICATION layer (like Order Service)
            var order = _orderService.GetOrder(context.Message.OrderId);

            var customer = _customerRepository.Get(context.Message.CustomerId);
            customer.Debit(order.TotalCost);


            await context.Publish<CustomerDebited>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });
        }
    }
}
