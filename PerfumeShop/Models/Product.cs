using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerfumeShop.Models;

/// <summary>
/// Sản phẩm nước hoa nam.
/// </summary>
public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Thương hiệu, ví dụ: Dior, Chanel, Versace.</summary>
    [MaxLength(100)]
    public string? Brand { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>Nốt hương chính, cách nhau bởi dấu phẩy. VD: "Bergamot, Amber".</summary>
    [MaxLength(200)]
    public string? Notes { get; set; }

    /// <summary>Giá bán (VND). Dùng decimal để chính xác về tiền tệ.</summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>Dung tích (ml), ví dụ 50, 100.</summary>
    public int VolumeMl { get; set; }

    /// <summary>Số lượng tồn kho.</summary>
    public int Stock { get; set; }

    /// <summary>Đường dẫn ảnh sản phẩm (URL hoặc file trong wwwroot).</summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Khóa ngoại tới danh mục
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
