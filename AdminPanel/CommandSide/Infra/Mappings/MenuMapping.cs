using AdminPanel.CommandSide.Domain.Menu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminPanel.CommandSide.Infra.Mappings
{
    public class MenuMapping : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.HasMany(c => c.Children).WithOne().HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(c => new { c.ParentId, c.Title }).IsUnique();
        }
    }
}
