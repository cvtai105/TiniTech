using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configurations;

public class VariantMetricConfiguration : IEntityTypeConfiguration<VariantMetric>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<VariantMetric> builder)
    {
    }
}
