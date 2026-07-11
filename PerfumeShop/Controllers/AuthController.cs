using Microsoft.AspNetCore.Mvc;
using PerfumeShop.DTOs;
using PerfumeShop.Services;

namespace PerfumeShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Đăng ký tài khoản mới (mặc định vai trò User).</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (result is null)
            return Conflict(new { message = "Email đã được sử dụng." });

        return Ok(result);
    }

    /// <summary>Đăng nhập, trả về JWT nếu thành công.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (result is null)
            return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

        return Ok(result);
    }
}
