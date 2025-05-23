using SharedKernel.Enums;

namespace WebSharedModels.Dtos.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    public int? ParentId { get; set; }
    public List<CategoryDto> Subcategories { get; set; } = new List<CategoryDto>();
}