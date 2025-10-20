using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQLServer.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Items>
    {
        public void Configure(EntityTypeBuilder<Items> builder)
        {
            builder.ToTable("Items", "blog");

            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Quantity).IsRequired().HasPrecision(18, 2);
            builder.Property(u => u.Description).HasMaxLength(500);
            builder.HasOne(u => u.MeasurementUnit).WithMany().HasForeignKey("MeasurementUnitId");
        }
    }
}
