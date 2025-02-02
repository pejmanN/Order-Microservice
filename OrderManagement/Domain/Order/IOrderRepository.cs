using Framework.Core;

namespace OrderManagement.Domain.Order
{
    public interface IOrderRepository : IRepository
    {
        void Add(Order order);
        Task SaveChangesAsync();
        long GetNextId();

        Order Get(long orderId);

    }
}
