using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.Models;

/// <summary>
/// Danh mục nước hoa (ví dụ: Nước hoa nam công sở, thể thao, buổi tối...).
/// </summary>
public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Quan hệ: một danh mục có nhiều sản phẩm
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
