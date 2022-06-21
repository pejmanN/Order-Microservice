using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Order;

namespace OrderManagement.Infra.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Order order)
        {
            _dbContext.Orders.Add(order);
        }

        public long GetNextId()
        {
            var param = new SqlParameter("@result", System.Data.SqlDbType.Int);
            param.Direction = System.Data.ParameterDirection.Output;
            _dbContext.Database.ExecuteSqlRaw("set @result = next value for OrderSequence", param);
            return (int)param.Value;
        }
        public async Task AsyncSaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
