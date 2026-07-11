using System.ComponentModel.DataAnnotations;

namespace PerfumeShop.Models;

/// <summary>
/// Tài khoản người dùng (khách hàng hoặc admin).
/// </summary>
public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    /// <summary>Mật khẩu đã được băm (hash), không lưu mật khẩu gốc.</summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Quan hệ: mỗi user có một giỏ hàng
    public Cart? Cart { get; set; }
}
