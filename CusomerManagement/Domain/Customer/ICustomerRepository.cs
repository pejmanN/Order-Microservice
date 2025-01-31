using Framework.Core;

namespace CusomerManagement.Domain.Customer
{
    public interface ICustomerRepository : IRepository
    {
        void Add(Customer customer);
        Customer Get(Guid id);
    }
}
