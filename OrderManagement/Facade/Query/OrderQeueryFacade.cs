using Microsoft.EntityFrameworkCore;
using OrderManagement.Infra.Query;

namespace OrderManagement.Facade.Query
{
    public class OrderQeueryFacade : IOrderQeueryFacade
    {
        private readonly QueryDbContext queryDbContext;

        public OrderQeueryFacade(QueryDbContext queryDbContext)
        {
            this.queryDbContext = queryDbContext;
        }

        public Order GetOrder(Guid correlationId)
        {
            var order = queryDbContext.Orders
               .Include(x => x.OrderLines)
               .FirstOrDefault(order => order.CorrelaitonId == correlationId);

            return order;
        }
    }
}
