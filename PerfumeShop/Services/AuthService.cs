using Microsoft.EntityFrameworkCore;
using PerfumeShop.Data;
using PerfumeShop.DTOs;
using PerfumeShop.Models;

namespace PerfumeShop.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}

/// <summary>
/// Xử lý đăng ký / đăng nhập, băm mật khẩu và tạo token.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    /// <returns>null nếu email đã tồn tại.</returns>
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await _db.Users.AnyAsync(u => u.Email == email))
            return null;

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.User
        };

        // Tạo sẵn giỏ hàng rỗng cho người dùng mới
        user.Cart = new Cart();

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return BuildResponse(user);
    }

    /// <returns>null nếu sai email hoặc mật khẩu.</returns>
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        return BuildResponse(user);
    }

    private AuthResponse BuildResponse(User user) => new()
    {
        Token = _tokenService.CreateToken(user),
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role.ToString()
    };
}
