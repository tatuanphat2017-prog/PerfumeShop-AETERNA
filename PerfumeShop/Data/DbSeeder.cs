using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Data;

/// <summary>
/// Nạp dữ liệu khởi tạo: tài khoản admin mặc định, danh mục và vài sản phẩm mẫu.
/// Chỉ thêm khi bảng còn trống để tránh trùng lặp.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Áp dụng migration còn thiếu (tiện khi chạy lần đầu)
        await db.Database.MigrateAsync();

        // 1) Tài khoản admin mặc định
        if (!await db.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            db.Users.Add(new User
            {
                FullName = "Quản trị viên",
                Email = "admin@perfume.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin
            });
        }

        // 2) Danh mục + sản phẩm mẫu
        if (!await db.Categories.AnyAsync())
        {
            var congSo = new Category { Name = "Công sở", Description = "Hương nhẹ nhàng, lịch lãm cho môi trường công sở." };
            var theThao = new Category { Name = "Thể thao", Description = "Hương tươi mát, năng động." };
            var buoiToi = new Category { Name = "Buổi tối", Description = "Hương nồng ấm, quyến rũ cho buổi hẹn." };
            db.Categories.AddRange(congSo, theThao, buoiToi);

            // Hàm rút gọn tạo sản phẩm
            static Product P(string name, string brand, string desc, string notes,
                decimal price, int vol, int stock, string imgId, Category cat) => new()
            {
                Name = name, Brand = brand, Description = desc, Notes = notes,
                Price = price, VolumeMl = vol, Stock = stock,
                ImageUrl = $"https://images.pexels.com/photos/{imgId}/pexels-photo-{imgId}.jpeg?auto=compress&cs=tinysrgb&w=600",
                Category = cat
            };

            db.Products.AddRange(
                P("Dior Sauvage EDT", "Dior", "Hương thơm nam tính, tươi mát với cam bergamot và tiêu Sichuan.", "Bergamot, Tiêu Sichuan", 2650000, 100, 20, "30970928", congSo),
                P("Dior Sauvage EDP", "Dior", "Phiên bản đậm đặc, nam tính và cuốn hút với cam bergamot hòa cùng ambroxan ấm nồng.", "Bergamot, Ambroxan, Vani", 2950000, 100, 25, "17978253", congSo),
                P("Bleu de Chanel EDP", "Chanel", "Biểu tượng thanh lịch: tươi mát với cam và gừng, kết thúc bằng gỗ đàn hương sang trọng.", "Cam, Gừng, Gỗ đàn hương", 3150000, 100, 20, "29805437", congSo),
                P("Versace Eros EDT", "Versace", "Hương bạc hà và táo xanh tươi mát quyện với vani ấm áp, mạnh mẽ và đầy đam mê.", "Bạc hà, Táo xanh, Vani", 1850000, 100, 30, "33820353", buoiToi),
                P("Creed Aventus", "Creed", "Huyền thoại niche với dứa tươi, khói bạch dương và xạ hương, dành cho quý ông thành đạt.", "Dứa, Bạch dương, Xạ hương", 8900000, 100, 8, "10191850", buoiToi),
                P("Dolce & Gabbana The One EDP", "Dolce & Gabbana", "Hương phương Đông ấm áp với thuốc lá, gừng và tuyết tùng, lịch lãm và quyến rũ.", "Thuốc lá, Gừng, Tuyết tùng", 2250000, 100, 18, "9202888", buoiToi),
                P("Paco Rabanne 1 Million", "Paco Rabanne", "Hương bưởi, quế và da thuộc gợi cảm giác xa hoa, cá tính và bốc lửa.", "Bưởi, Quế, Da thuộc", 2050000, 100, 22, "14211239", buoiToi),
                P("YSL La Nuit de L'Homme", "Yves Saint Laurent", "Bạch đậu khấu quyện tuyết tùng và oải hương tạo nên sự nam tính bí ẩn cho buổi tối.", "Bạch đậu khấu, Tuyết tùng, Oải hương", 2400000, 100, 16, "14490682", buoiToi),
                P("Acqua di Gio Profondo", "Giorgio Armani", "Hương biển sâu lắng với bergamot và khoáng chất, tươi mát và hiện đại.", "Hương biển, Bergamot, Xạ hương", 2600000, 125, 20, "30990130", theThao),
                P("Allure Homme Sport", "Chanel", "Năng động và tinh tế: cam, tiêu và đậu tonka mang lại năng lượng tươi mới.", "Cam, Tiêu, Đậu tonka", 2700000, 100, 15, "3640668", theThao),
                P("Montblanc Explorer", "Montblanc", "Bergamot Ý, da thuộc và hoắc hương Indonesia cho quý ông ưa khám phá.", "Bergamot, Da thuộc, Hoắc hương", 1500000, 100, 26, "21614757", theThao),
                P("Hugo Boss Bottled", "Hugo Boss", "Táo, quế và gỗ tạo nên mùi hương của người đàn ông của công việc — chỉn chu, đáng tin.", "Táo, Quế, Gỗ", 1650000, 100, 24, "14687026", congSo),
                P("Prada L'Homme", "Prada", "Diên vĩ, phong lữ và tuyết tùng mang lại vẻ lịch lãm nhã nhặn, rất hợp công sở.", "Diên vĩ, Phong lữ, Tuyết tùng", 2500000, 100, 14, "34154866", congSo),
                P("Tom Ford Oud Wood", "Tom Ford", "Trầm hương quý phái quyện gỗ hồng và bạch đậu khấu — sang trọng và khác biệt.", "Trầm hương, Gỗ hồng, Bạch đậu khấu", 6500000, 100, 10, "15260901", buoiToi),
                P("Tom Ford Tobacco Vanille", "Tom Ford", "Thuốc lá và vani ấm áp, ngọt ngào và ấm cúng cho mùa lạnh.", "Thuốc lá, Vani, Gia vị", 6900000, 100, 9, "11860930", buoiToi),
                P("JPG Le Male", "Jean Paul Gaultier", "Bạc hà, oải hương và vani — mùi hương biểu tượng vừa nam tính vừa ngọt ngào.", "Bạc hà, Oải hương, Vani", 1900000, 125, 20, "10415082", theThao),
                P("Nautica Voyage", "Nautica", "Hương biển tươi mát với táo và gỗ, giá tốt — lựa chọn quen thuộc của sinh viên.", "Táo, Hương biển, Gỗ", 450000, 100, 40, "1653085", theThao),
                P("Davidoff Cool Water", "Davidoff", "Bạc hà và oải hương mát lạnh cùng xạ hương biển — kinh điển, dễ dùng hằng ngày.", "Bạc hà, Oải hương, Xạ hương biển", 700000, 125, 35, "16085365", theThao),
                P("Bvlgari Man In Black", "Bvlgari", "Rượu rum, hoa cam và da thuộc tạo nên nét đàn ông trưởng thành, quyến rũ.", "Rượu rum, Hoa cam, Da thuộc", 2300000, 100, 15, "7751714", buoiToi),
                P("Azzaro Wanted", "Azzaro", "Chanh, bạch đậu khấu và gỗ khô mang lại sự tươi mới, tự tin cho ngày dài.", "Chanh, Bạch đậu khấu, Gỗ", 1700000, 100, 21, "8166613", congSo),
                P("Gucci Guilty Pour Homme", "Gucci", "Oải hương, cam Ý và hoắc hương — hiện đại, phóng khoáng và cuốn hút.", "Oải hương, Cam Ý, Hoắc hương", 2150000, 90, 17, "954405", congSo),
                P("Burberry Hero", "Burberry", "Bergamot, tuyết tùng và nhựa thơm gợi hình ảnh người đàn ông tự do, mạnh mẽ.", "Bergamot, Tuyết tùng, Nhựa thơm", 2000000, 100, 19, "2814832", congSo),
                P("Calvin Klein CK One", "Calvin Klein", "Cam chanh, trà xanh và xạ hương — hương unisex nhẹ nhàng, trẻ trung.", "Cam chanh, Trà xanh, Xạ hương", 850000, 100, 33, "3785784", theThao),
                P("Montblanc Legend", "Montblanc", "Oải hương, táo và rêu sồi tạo nên nét lịch lãm cổ điển, đáng tin cậy.", "Oải hương, Táo, Rêu sồi", 1600000, 100, 23, "11711813", congSo),
                P("Lacoste L.12.12 Blanc", "Lacoste", "Bưởi, cây bách và tuyết tùng — sạch sẽ, tươi mát, phù hợp mọi lứa tuổi.", "Bưởi, Cây bách, Tuyết tùng", 1250000, 100, 28, "6947684", theThao)
            );
        }

        await db.SaveChangesAsync();
    }
}
