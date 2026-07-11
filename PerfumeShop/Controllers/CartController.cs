using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.DTOs;
using PerfumeShop.Services;

namespace PerfumeShop.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Phải đăng nhập mới dùng được giỏ hàng
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>Lấy Id người dùng hiện tại từ token.</summary>
    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Xem giỏ hàng của tôi.</summary>
    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetMyCart()
    {
        return Ok(await _cartService.GetCartAsync(CurrentUserId));
    }

    /// <summary>Thêm sản phẩm vào giỏ.</summary>
    [HttpPost("items")]
    public async Task<ActionResult<CartResponse>> AddItem(AddToCartRequest request)
    {
        var result = await _cartService.AddItemAsync(CurrentUserId, request);
        return result is null ? NotFound(new { message = "Sản phẩm không tồn tại." }) : Ok(result);
    }

    /// <summary>Cập nhật số lượng một sản phẩm trong giỏ (0 = xóa).</summary>
    [HttpPut("items")]
    public async Task<ActionResult<CartResponse>> UpdateItem(UpdateCartItemRequest request)
    {
        var result = await _cartService.UpdateItemAsync(CurrentUserId, request);
        return result is null ? NotFound(new { message = "Sản phẩm không có trong giỏ." }) : Ok(result);
    }

    /// <summary>Xóa một sản phẩm khỏi giỏ.</summary>
    [HttpDelete("items/{productId:int}")]
    public async Task<ActionResult<CartResponse>> RemoveItem(int productId)
    {
        var result = await _cartService.RemoveItemAsync(CurrentUserId, productId);
        return result is null ? NotFound(new { message = "Sản phẩm không có trong giỏ." }) : Ok(result);
    }

    /// <summary>Làm rỗng giỏ hàng.</summary>
    [HttpDelete]
    public async Task<ActionResult<CartResponse>> Clear()
    {
        return Ok(await _cartService.ClearAsync(CurrentUserId));
    }
}
