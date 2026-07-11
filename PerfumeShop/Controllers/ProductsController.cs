using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.DTOs;
using PerfumeShop.Services;

namespace PerfumeShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>Danh sách sản phẩm, có thể lọc theo từ khóa và danh mục. Ai cũng xem được.</summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? categoryId)
    {
        return Ok(await _productService.GetAllAsync(search, categoryId));
    }

    /// <summary>Chi tiết một sản phẩm.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Tạo sản phẩm mới. Chỉ Admin.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> Create(ProductCreateRequest request)
    {
        var created = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Cập nhật sản phẩm. Chỉ Admin.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> Update(int id, ProductUpdateRequest request)
    {
        var updated = await _productService.UpdateAsync(id, request);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Xóa sản phẩm. Chỉ Admin.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _productService.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
