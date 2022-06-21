using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Order;

namespace OrderManagement.Infra.Persistence.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever();
            builder.Property(a => a.CustomerId);
            builder.Property(a => a.IssueDate);

            builder.OwnsMany(a => a.OrderLines, map =>
            {
                map.ToTable("OrderLines").HasKey("Id");
                map.Property<long>("Id").ValueGeneratedOnAdd();
                map.WithOwner().HasForeignKey("OrderId");

                map.Property(a => a.EachPrice);
                map.Property(a => a.ProductId);
                map.Property(a => a.Quantity);
                map.UsePropertyAccessMode(PropertyAccessMode.Field);
            });
        }
    }
}
