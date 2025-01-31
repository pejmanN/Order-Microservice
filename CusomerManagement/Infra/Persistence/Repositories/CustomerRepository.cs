using CusomerManagement.Domain.Customer;

namespace CusomerManagement.Infra.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public void Add(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer Get(long id)
        {
            return new Customer(1, "Bob", false, 1000);
        }
    }
}
