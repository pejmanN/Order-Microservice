using Framework.Domain.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Outbox.Infra.Persistence
{
    public class OrderOutboxDbContext : DbContext
    {
        public OrderOutboxDbContext(DbContextOptions<OrderOutboxDbContext> options) : base(options)
        {
        }

        public DbSet<DomainEventItem> DomainEventItems { get; set; }
    }
}
