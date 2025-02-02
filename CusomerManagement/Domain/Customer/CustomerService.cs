using CusomerManagement.Domain.Service;

namespace CusomerManagement.Domain.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public bool CusotmerIsValid(Guid CustomerId)
        {
            var customer = _customerRepository.Get(CustomerId);
            //any business rule 
            //if (customer is null || customer.Disabled == true)
            //{
            //    return false;
            //}
            return true;
        }
    }
}
