using Framework.Domain;
using Framework.Domain.EventOutbox;
using Microsoft.EntityFrameworkCore;


namespace OrderManagement.Infra.Query
{
    public class QueryDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }


        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            throw new NotSupportedException();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
