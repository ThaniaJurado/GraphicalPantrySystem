using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQLServer.Data.Configurations
{
    public class MeasurementUnitConfiguration : IEntityTypeConfiguration<Items>
    {
        public void Configure(EntityTypeBuilder<Items> builder)
        {
            builder.ToTable("MeasurementUnits", "blog");

            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Description).HasMaxLength(200);
        }
    }
}
