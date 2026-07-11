using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.DTOs;

public class AddToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemRequest
{
    [Required]
    public int ProductId { get; set; }

    /// <summary>Số lượng mới (đặt tuyệt đối). Bằng 0 sẽ xóa khỏi giỏ.</summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}

public class CartItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
    public string? ImageUrl { get; set; }
}

public class CartResponse
{
    public List<CartItemResponse> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.LineTotal);
}
