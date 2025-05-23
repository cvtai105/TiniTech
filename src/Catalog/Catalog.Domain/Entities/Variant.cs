namespace Catalog.Domain.Entities;

public class Variant : BaseAuditableEntity
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public string Sku { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public Product Product { get; set; } = null!;
    public virtual ICollection<VariantAttribute> VariantAttributes { get; set; } = [];
    public VariantMetric Metric { get; set; } = null!;
}

