using CusomerManagement.Domain.Customer;

namespace CusomerManagement.Infra.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public void Add(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer Get(Guid id)
        {
            return new Customer("Bob", false, 1000);
        }
    }
}
