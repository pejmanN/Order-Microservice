using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using MassTransit;
using Shared.StateMachines.Order.Models;

namespace CusomerManagement.Infra.Consumers
{
    public class DebitCustomerConsumer : IConsumer<DebitCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderService _orderService;
        private readonly ILogger<DebitCustomerConsumer> _logger;

        public DebitCustomerConsumer(ICustomerRepository customerRepository,
                                    IOrderService orderService,
                                    ILogger<DebitCustomerConsumer> logger)
        {
            _customerRepository = customerRepository;
            _orderService = orderService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<DebitCustomer> context)
        {
            //NOTE :Business Login should move to APPLICATION layer (like Order Service) and Event published using outbox pattern
            //var order = _orderService.GetOrder(context.Message.OrderId);
            //var customer = _customerRepository.Get(context.Message.CustomerId);
            //customer.Debit(order.TotalCost,order.OrderId);

            _logger.LogInformation("CustomerDebitConsumer for Customer {context.Message.CustomerId}, Order ={context.Message.OrderId}",
                context.Message.CustomerId, context.Message.OrderId);
            await context.Publish<CustomerDebited>(new
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
            });
        }
    }
}
