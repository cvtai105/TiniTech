using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations;

public class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {
        builder.Property(v => v.Sku)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(v => v.VariantAttributes)
            .WithOne(va => va.Variant)
            .HasForeignKey(va => va.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();
    }
}
