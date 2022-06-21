using Framework.Domain;
using Framework.Domain.EventOutbox;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Order;
using OrderManagement.Infra.Persistence.Mappings;

namespace OrderManagement.Infra.Persistence
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<DomainEventItem> DomainEventItems { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            this.SavingChanges += OnSavingChanges;
        }

        private void OnSavingChanges(object sender, SavingChangesEventArgs e)
        {
            var aggregateRoots = this.ChangeTracker.Entries()
                    .Where(a => a.State != EntityState.Unchanged)
                    .Select(a => a.Entity)
                    .OfType<IAggregateRoot>()
                    .ToList();

            var itemsToAddIntoOutbox = EventItemFactory.CreateOutboxItemsFromAggregateRoots(aggregateRoots);
            itemsToAddIntoOutbox.ForEach(a => DomainEventItems.Add(a));
            aggregateRoots.ForEach(x => x.ClearUncommittedEvents());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO:type should be chnaged from int to long
            modelBuilder.HasSequence<int>("OrderSequence").StartsAt(1).IncrementsBy(1).IsCyclic(false);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
