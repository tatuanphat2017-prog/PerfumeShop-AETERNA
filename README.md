# AETERNA — Web bán nước hoa nam

Đồ án cá nhân: website bán nước hoa nam với backend ASP.NET Core Web API, database SQL Server (EF Core), phân quyền Admin/User, và frontend HTML/CSS/JS thuần.

🔗 **Live demo:** http://ttp-aeterna.runasp.net
👤 **Admin demo:** `admin@perfume.com` / `Admin@123`

> Lưu ý: file `appsettings.Production.json` (chứa cấu hình bí mật) không được đưa lên Git. Khi tự chạy, tạo file này với connection string SQL Server của bạn.

## Công nghệ

| Thành phần | Công nghệ |
|-----------|-----------|
| Backend   | ASP.NET Core 8 Web API (C#) |
| Database  | SQL Server + Entity Framework Core |
| Xác thực  | JWT + BCrypt (băm mật khẩu) |
| Frontend  | HTML / CSS / JavaScript thuần (gọi API bằng fetch) |
| Tài liệu API | Swagger |

## Cấu trúc thư mục

```
PerfumeShop/
├── Controllers/     # API endpoints (Auth, Products, Categories, Cart)
├── Services/        # Tầng nghiệp vụ (tách khỏi controller)
├── Models/          # Entity: User, Product, Category, Cart, CartItem
├── DTOs/            # Đối tượng truyền dữ liệu client <-> API
├── Data/            # DbContext + seed dữ liệu mẫu
├── wwwroot/         # Frontend tĩnh
├── Program.cs       # Cấu hình & khởi động ứng dụng
└── appsettings.json # Connection string + cấu hình JWT
```

## Chạy trên máy (local)

1. Mở `PerfumeShop.sln` bằng **Visual Studio 2022**.
2. Kiểm tra chuỗi kết nối trong `appsettings.json` cho khớp SQL Server của bạn.
   - Dùng SQL Server Express: `Server=localhost\SQLEXPRESS;...`
   - Dùng LocalDB: đổi thành `Server=(localdb)\MSSQLLocalDB;...`
3. Mở **Package Manager Console** và tạo database:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```
   (Ứng dụng cũng tự chạy migration khi khởi động.)
4. Nhấn **F5** để chạy. Trình duyệt mở tại `/swagger` để test API.

### Tài khoản admin mẫu
- Email: `admin@perfume.com`
- Mật khẩu: `Cung cấp khi bàn giao`

## Bảo mật cần lưu ý trước khi deploy
- Đổi `Jwt:Key` trong `appsettings.json` thành chuỗi bí mật dài, ngẫu nhiên.
- Đổi mật khẩu admin mặc định.

## API chính

| Method | Endpoint | Quyền | Mô tả |
|--------|----------|-------|-------|
| POST | `/api/auth/register` | Công khai | Đăng ký |
| POST | `/api/auth/login` | Công khai | Đăng nhập, trả JWT |
| GET  | `/api/products` | Công khai | Danh sách sản phẩm (lọc `search`, `categoryId`) |
| GET  | `/api/products/{id}` | Công khai | Chi tiết sản phẩm |
| POST | `/api/products` | Admin | Thêm sản phẩm |
| PUT  | `/api/products/{id}` | Admin | Sửa sản phẩm |
| DELETE | `/api/products/{id}` | Admin | Xóa sản phẩm |
| GET  | `/api/categories` | Công khai | Danh sách danh mục |
| POST | `/api/categories` | Admin | Thêm danh mục |
| GET  | `/api/cart` | Đăng nhập | Xem giỏ hàng |
| POST | `/api/cart/items` | Đăng nhập | Thêm vào giỏ |
| DELETE | `/api/cart/items/{productId}` | Đăng nhập | Xóa khỏi giỏ |
| DELETE | `/api/cart` | Đăng nhập | Làm rỗng giỏ |
