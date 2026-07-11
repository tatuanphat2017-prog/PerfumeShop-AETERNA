using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.DTOs;

/// <summary>Dữ liệu sản phẩm trả về cho client.</summary>
public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public decimal Price { get; set; }
    public int VolumeMl { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

/// <summary>Dữ liệu tạo mới sản phẩm (chỉ admin).</summary>
public class ProductCreateRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Brand { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>Nốt hương, cách nhau bởi dấu phẩy. VD: "Bergamot, Amber".</summary>
    [MaxLength(200)]
    public string? Notes { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int VolumeMl { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }
}

/// <summary>Dữ liệu cập nhật sản phẩm (chỉ admin). Cùng cấu trúc với tạo mới.</summary>
public class ProductUpdateRequest : ProductCreateRequest
{
}
