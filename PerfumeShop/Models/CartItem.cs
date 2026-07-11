namespace PerfumeShop.Models;

/// <summary>
/// Một dòng trong giỏ hàng: một sản phẩm và số lượng.
/// </summary>
public class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
}
