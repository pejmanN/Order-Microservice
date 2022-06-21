using AdminPanel.CommandSide.Domain.Menu;
using AdminPanel.CommandSide.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.CommandSide.Infra
{
    public class PanelDbContextt : DbContext
    {
        public DbSet<Menu> Menus { get; set; }

        public PanelDbContextt(DbContextOptions<PanelDbContextt> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("MenuSequence").StartsAt(1).IncrementsBy(1).IsCyclic(false);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
