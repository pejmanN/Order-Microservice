using Framework.Core;

namespace OrderManagement.Domain.Order
{
    public interface IOrderRepository : IRepository
    {
        void Add(Order order);
        Task AsyncSaveChanges();
        long GetNextId();

    }
}
