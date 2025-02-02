using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Order;

namespace OrderManagement.Infra.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _dbContext;

        public OrderRepository(OrderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Order order)
        {
            _dbContext.Orders.Add(order);
        }

        public Order Get(long orderId)
        {
            return _dbContext.Orders.FirstOrDefault(order => order.Id == orderId);
        }

        public long GetNextId()
        {
            var param = new SqlParameter("@result", System.Data.SqlDbType.Int);
            param.Direction = System.Data.ParameterDirection.Output;
            _dbContext.Database.ExecuteSqlRaw("set @result = next value for OrderSequence", param);
            return (int)param.Value;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
