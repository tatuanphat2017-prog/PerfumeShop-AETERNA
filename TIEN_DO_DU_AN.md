# Tiến độ dự án — Web bán nước hoa nam (AETERNA)

> Ghi chú để tiếp tục làm vào hôm sau. Cập nhật lần cuối: phiên làm việc đổi tông màu xanh lam.

## ✅ ĐÃ DEPLOY ONLINE (hoàn tất)
- Web chạy thật tại: **http://ttp-aeterna.runasp.net**
- Hosting: **MonsterASP.NET** (gói free, .NET 8 + SQL Server), deploy bằng Web Deploy (Publish Profile) từ Visual Studio.
- Cấu hình online nằm ở `appsettings.Production.json` (connection string DB MonsterASP + khóa JWT mạnh). Local vẫn dùng `appsettings.json` như cũ.
- Database online đã tự tạo bảng + nạp 25 sản phẩm + admin nhờ seeder.
- CÒN LẠI: **HTTPS** — gói free cần gửi Support ticket cho MonsterASP để bật Let's Encrypt; sau khi bật thì Enable HTTPS + Force redirect. (Web vẫn dùng tốt qua http.)
- Muốn cập nhật web sau này: sửa code → trong Visual Studio chuột phải project → Publish → bấm Publish lại (dùng profile đã lưu).

## Tài liệu kèm theo (trong thư mục Project_caNhan)
- `CONG_NGHE_SU_DUNG.txt` — tổng hợp công nghệ (dùng cho CV).
- `HUONG_DAN_SU_DUNG.txt` — hướng dẫn sử dụng cho khách/admin.
- `TIEN_DO_DU_AN.md` — file này.

## ⚠️ Ghi chú kỹ thuật cũ (tham khảo)
Đã thêm cột **Notes (nốt hương)** vào bảng Product → **phải chạy migration**:
```powershell
Add-Migration AddProductNotes
Update-Database
```
Sau đó F5. Nếu CSS/JS bị cache thì Ctrl+F5.
(3 sản phẩm cũ sẽ có Notes = null → vào Quản trị > Sửa để nhập nốt hương, hoặc xóa DB tạo lại để seed có sẵn.)

## Đã làm trong phiên tinh chỉnh Stitch
- Thêm trường **Notes** (nốt hương) vào Product (model, DTO, service, seed).
- Thẻ sản phẩm: hiện **nhãn nốt hương** (chip) trên ảnh; form Quản trị có ô nhập Notes; trang chi tiết hiện tag.
- **Thanh lọc danh mục** kiểu Stitch: chữ + chấm gold dưới mục đang chọn (`.cat-btn` trong shop.html).
- **Icon Material Symbols** trong header (search, person/logout, shopping_bag, menu) + footer (mail, location, public). Font nhúng qua `ensureIconFont()` trong app.js.
- **Layout bento** ở trang Giới thiệu (The Heart of AETERNA): 1 ô lớn + 3 ô nhỏ.

## Phiên dữ liệu + polish giao diện (mới nhất)
- Đã thêm **24 sản phẩm** nước hoa nam thật (script SQL). Ảnh dùng **Pexels** (mỗi SP một ảnh riêng — trước đó Unsplash bị lỗi do trùng ảnh).
  → File `seed_products.sql` (thêm SP) và câu UPDATE ảnh Pexels đã chạy trong SSMS.
- Thẻ sản phẩm: **ảnh vuông 1:1**, nút **"Thêm vào giỏ"** dạng thanh trượt, sản phẩm **hết hàng** làm mờ + gạch giá.
- **Header thu gọn + đổ bóng khi cuộn**; **fade chuyển trang** khi bấm link nội bộ.
- Trang Cửa hàng: **sắp xếp theo giá** (Mới nhất / Thấp→Cao / Cao→Thấp) + đếm số sản phẩm.
- Trang chi tiết: **đánh giá sao** (trang trí) + **sản phẩm liên quan** cùng danh mục.
- Các thay đổi này là frontend (wwwroot) → chỉ cần Ctrl+F5, KHÔNG cần migration.
- **Hero trang chủ:** đổi sang kiểu điện ảnh (ảnh tối + lớp phủ + chữ trắng + nút gold), và nâng thành **slideshow 5 ảnh** tự chuyển mỗi 3 giây (crossfade + chấm chỉ số). Code trong `index.html` (hàm initHeroSlideshow) + CSS `.hero-slide/.hero-dots`.

## Phiên rà soát & polish các trang (mới nhất)
- Đã QUÉT tất cả trang: không có lỗi chức năng.
- Sửa lỗi ảnh Unsplash dễ vỡ ở trang Giới thiệu → Pexels + fallback chống vỡ (thêm CSS `.atelier-media`).
- Chuẩn hóa: thông tin liên hệ/chi nhánh đổi thành placeholder `[ ]` để chủ shop tự điền; Việt hóa nhãn chức năng (Gửi tin nhắn, Liên hệ).
- Định hướng đã chốt với chủ dự án: ngôn ngữ **lai** (brand tiếng Anh + nội dung Việt); phong cách **sang trọng tối giản**.
- Polish 3 trang:
  - **Đăng nhập/Đăng ký** (`account.html`): bố cục 2 cột, ảnh minh họa bên trái + form bên phải (CSS `.auth-split/.auth-visual`).
  - **Giỏ hàng** (`cart.html`): dòng đếm sản phẩm, tóm tắt có 'Số sản phẩm', link 'Tiếp tục mua sắm', trạng thái trống có icon.
  - **Chi tiết sản phẩm** (`product.html`): breadcrumb, giá + nhãn nốt hương, mục 'Mô tả', bảng thông số `.spec-list`, khu vực mua `.buy-row`.
- Tất cả là frontend → chỉ Ctrl+F5, KHÔNG cần migration.

## Thông tin chung
- **Sinh viên:** Tuấn Phát (HUTECH, ngành CNTT — chuyên ngành CN Phần mềm)
- **Mục tiêu:** đồ án cá nhân để lấy kinh nghiệm ghi CV
- **Thư mục dự án:** `C:\Users\PHAT\Project_caNhan`
- **Mở bằng:** Visual Studio 2022 → file `PerfumeShop.sln`

## Tech stack đã chốt
- Backend: **ASP.NET Core 8 Web API (C#)**
- Database: **SQL Server** + Entity Framework Core
- Xác thực: **JWT** + BCrypt (băm mật khẩu)
- Frontend: **HTML/CSS/JS thuần** (đặt trong `wwwroot`, gọi API bằng `fetch`)
- Phong cách giao diện: boutique "AETERNA" theo mẫu Stitch, đã đổi sang **nền xanh lam, nút đen chữ trắng, nút phụ gold**

## Connection string (máy của Phát)
`Server=PHATPC\MSSQLSERVER02;Database=PerfumeShopDb;...` (đã cấu hình trong `appsettings.json`)

## Tài khoản mẫu
- Admin: `admin@perfume.com` / `Admin@123`
- User đã đăng ký: `tatuanphat.2017@gmail.com` (mật khẩu do Phát đặt)

## ĐÃ HOÀN THÀNH ✅
1. Backend đầy đủ: Models, DbContext + seed, Auth (JWT + phân quyền Admin/User), Controllers (Products CRUD, Categories, Cart).
   - Đã test trên Swagger: đăng ký/đăng nhập chạy đúng.
2. Frontend đầy đủ trong `wwwroot`:
   - `index.html` (trang chủ), `shop.html` (cửa hàng + lọc/tìm kiếm), `product.html` (chi tiết),
     `cart.html` (giỏ hàng), `account.html` (đăng nhập/đăng ký), `admin.html` (quản trị CRUD),
     `about.html`, `contact.html`
   - `css/style.css` (design system dùng chung), `js/app.js` (gọi API, token, header/footer, tiền VND)
3. Hiệu ứng cuộn kiểu Apple: scroll-reveal (IntersectionObserver), parallax hero, stagger, prefers-reduced-motion.
4. Đổi tông màu: nền xanh lam `#e7eef8`, nút chính đen `#111318`, nút phụ gold `#c7a253`, viền navy `#14314f`.
   - Toàn bộ biến màu nằm ở đầu `css/style.css` (`:root`).
5. Đã thêm endpoint `PUT /api/cart/items` để chỉnh số lượng trong giỏ.

## Lưu ý kỹ thuật
- KHÔNG cần tạo migration mới cho các thay đổi frontend/màu (không đổi cấu trúc DB).
- 3 sản phẩm seed ban đầu (trên DB của Phát) chưa có ảnh vì ImageUrl được thêm vào seed sau
  → có thể vào trang Quản trị → Sửa để dán link ảnh, hoặc xóa DB tạo lại để seed có ảnh.
- Chạy: nhấn F5 trong Visual Studio → trình duyệt mở thẳng trang chủ. Nếu CSS bị cache thì Ctrl+F5.

## CÒN LẠI / LÀM TIẾP 🔜
1. **(Tùy chọn) Bám sát mẫu Stitch hơn:** thêm nhãn nốt hương trên thẻ sản phẩm (OUD, BERGAMOT…),
   thanh lọc danh mục có chấm gold, layout bento ở trang Giới thiệu, dùng icon Material Symbols.
2. **DEPLOY** (bước cuối, ưu tiên): đưa web + database lên hosting để người khác truy cập.
   - Cần bàn: chọn nơi deploy (ví dụ Azure, hoặc hosting hỗ trợ ASP.NET + SQL Server, hoặc
     đổi DB sang phương án có gói free). Nhớ đổi `Jwt:Key` và mật khẩu admin trước khi deploy.

## Việc đầu tiên nên làm hôm sau
Hỏi Phát: muốn (a) tinh chỉnh thêm giao diện theo Stitch, hay (b) bắt tay deploy luôn.
