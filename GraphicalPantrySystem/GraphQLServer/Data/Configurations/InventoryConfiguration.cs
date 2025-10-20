using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQLServer.Data.Configurations
{
    public class InventoryConfiguration: IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories", "blog");
            builder.Property(u => u.ExpirationDate).IsRequired();
            builder.HasOne(typeof(Items), "Item").WithMany().HasForeignKey("ItemId");
        }
    }
}
