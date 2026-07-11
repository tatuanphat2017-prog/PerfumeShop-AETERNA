using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;

namespace PerfumeShop.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetAllAsync(string? search, int? categoryId);
    Task<ProductResponse?> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(ProductCreateRequest request);
    Task<ProductResponse?> UpdateAsync(int id, ProductUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}

/// <summary>
/// Nghiệp vụ quản lý sản phẩm: đọc danh sách, chi tiết, và CRUD cho admin.
/// </summary>
public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductResponse>> GetAllAsync(string? search, int? categoryId)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Name.Contains(s) || (p.Brand != null && p.Brand.Contains(s)));
        }

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        // Map sang DTO trong bộ nhớ (EF Core không dịch được lời gọi hàm trong Select)
        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product is null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(ProductCreateRequest request)
    {
        var product = new Product
        {
            Name = request.Name.Trim(),
            Brand = request.Brand?.Trim(),
            Description = request.Description?.Trim(),
            Notes = request.Notes?.Trim(),
            Price = request.Price,
            VolumeMl = request.VolumeMl,
            Stock = request.Stock,
            ImageUrl = request.ImageUrl?.Trim(),
            CategoryId = request.CategoryId
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        // Nạp lại tên danh mục để trả về đầy đủ
        await _db.Entry(product).Reference(p => p.Category).LoadAsync();
        return ToResponse(product);
    }

    public async Task<ProductResponse?> UpdateAsync(int id, ProductUpdateRequest request)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null)
            return null;

        product.Name = request.Name.Trim();
        product.Brand = request.Brand?.Trim();
        product.Description = request.Description?.Trim();
        product.Notes = request.Notes?.Trim();
        product.Price = request.Price;
        product.VolumeMl = request.VolumeMl;
        product.Stock = request.Stock;
        product.ImageUrl = request.ImageUrl?.Trim();
        product.CategoryId = request.CategoryId;

        await _db.SaveChangesAsync();

        await _db.Entry(product).Reference(p => p.Category).LoadAsync();
        return ToResponse(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null)
            return false;

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    private static ProductResponse ToResponse(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Brand = p.Brand,
        Description = p.Description,
        Notes = p.Notes,
        Price = p.Price,
        VolumeMl = p.VolumeMl,
        Stock = p.Stock,
        ImageUrl = p.ImageUrl,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name
    };
}
