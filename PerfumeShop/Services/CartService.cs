using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;

namespace PerfumeShop.Services;

public interface ICartService
{
    Task<CartResponse> GetCartAsync(int userId);
    Task<CartResponse?> AddItemAsync(int userId, AddToCartRequest request);
    Task<CartResponse?> UpdateItemAsync(int userId, UpdateCartItemRequest request);
    Task<CartResponse?> RemoveItemAsync(int userId, int productId);
    Task<CartResponse> ClearAsync(int userId);
}

/// <summary>
/// Nghiệp vụ giỏ hàng: thêm, xóa, xem và làm rỗng giỏ theo từng người dùng.
/// </summary>
public class CartService : ICartService
{
    private readonly AppDbContext _db;

    public CartService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<CartResponse> GetCartAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        return ToResponse(cart);
    }

    /// <returns>null nếu sản phẩm không tồn tại.</returns>
    public async Task<CartResponse?> AddItemAsync(int userId, AddToCartRequest request)
    {
        var product = await _db.Products.FindAsync(request.ProductId);
        if (product is null)
            return null;

        var cart = await GetOrCreateCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

        if (item is null)
            cart.Items.Add(new CartItem { ProductId = request.ProductId, Quantity = request.Quantity });
        else
            item.Quantity += request.Quantity;

        await _db.SaveChangesAsync();
        return ToResponse(cart);
    }

    /// <summary>Đặt lại số lượng cho một sản phẩm trong giỏ (0 = xóa).</summary>
    /// <returns>null nếu sản phẩm không có trong giỏ.</returns>
    public async Task<CartResponse?> UpdateItemAsync(int userId, UpdateCartItemRequest request)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (item is null)
            return null;

        if (request.Quantity <= 0)
            cart.Items.Remove(item);
        else
            item.Quantity = request.Quantity;

        await _db.SaveChangesAsync();
        return ToResponse(cart);
    }

    /// <returns>null nếu sản phẩm không có trong giỏ.</returns>
    public async Task<CartResponse?> RemoveItemAsync(int userId, int productId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            return null;

        cart.Items.Remove(item);
        await _db.SaveChangesAsync();
        return ToResponse(cart);
    }

    public async Task<CartResponse> ClearAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        cart.Items.Clear();
        await _db.SaveChangesAsync();
        return ToResponse(cart);
    }

    private async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _db.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
        }

        return cart;
    }

    private static CartResponse ToResponse(Cart cart) => new()
    {
        Items = cart.Items.Select(i => new CartItemResponse
        {
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            UnitPrice = i.Product?.Price ?? 0,
            Quantity = i.Quantity,
            ImageUrl = i.Product?.ImageUrl
        }).ToList()
    };
}
