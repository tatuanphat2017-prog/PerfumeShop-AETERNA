namespace PerfumeShop.Models;

/// <summary>
/// Giỏ hàng của một người dùng.
/// </summary>
public class Cart
{
    public int Id { get; set; }

    // Khóa ngoại tới người dùng sở hữu giỏ hàng
    public int UserId { get; set; }
    public User? User { get; set; }

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
