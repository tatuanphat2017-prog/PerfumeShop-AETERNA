/* ============================================================
   AETERNA — Bộ dữ liệu 24 sản phẩm nước hoa nam
   Cách dùng:
     1. Mở file này trong SQL Server Management Studio / SSMS.
     2. Đảm bảo đang chọn database PerfumeShopDb (dòng USE bên dưới đã lo).
     3. Nhấn Execute (F5).
   An toàn: có kiểm tra NOT EXISTS nên chạy nhiều lần cũng không bị trùng.
   Mô tả & nốt hương do tự biên soạn; ảnh dùng nguồn royalty-free (Unsplash).
   ============================================================ */
USE PerfumeShopDb;
GO

SET NOCOUNT ON;

/* Hàm tiện: chèn 1 sản phẩm nếu chưa tồn tại (theo tên) */
DECLARE @congSo INT = (SELECT Id FROM dbo.Categories WHERE Name = N'Công sở');
DECLARE @theThao INT = (SELECT Id FROM dbo.Categories WHERE Name = N'Thể thao');
DECLARE @buoiToi INT = (SELECT Id FROM dbo.Categories WHERE Name = N'Buổi tối');

/* Nếu vì lý do nào đó chưa có danh mục, tạo mới cho đủ */
IF @congSo IS NULL BEGIN INSERT INTO dbo.Categories(Name,[Description]) VALUES(N'Công sở',N'Hương nhẹ nhàng, lịch lãm cho công sở.'); SET @congSo = SCOPE_IDENTITY(); END
IF @theThao IS NULL BEGIN INSERT INTO dbo.Categories(Name,[Description]) VALUES(N'Thể thao',N'Hương tươi mát, năng động.'); SET @theThao = SCOPE_IDENTITY(); END
IF @buoiToi IS NULL BEGIN INSERT INTO dbo.Categories(Name,[Description]) VALUES(N'Buổi tối',N'Hương nồng ấm, quyến rũ cho buổi hẹn.'); SET @buoiToi = SCOPE_IDENTITY(); END

/* Danh sách sản phẩm (bảng tạm) */
DECLARE @P TABLE (
  Name NVARCHAR(200), Brand NVARCHAR(100), Descr NVARCHAR(2000), Notes NVARCHAR(200),
  Price DECIMAL(18,2), VolumeMl INT, Stock INT, ImageUrl NVARCHAR(500), CategoryId INT
);

INSERT INTO @P (Name, Brand, Descr, Notes, Price, VolumeMl, Stock, ImageUrl, CategoryId) VALUES
(N'Dior Sauvage EDP', N'Dior', N'Phiên bản đậm đặc, nam tính và cuốn hút với cam bergamot tươi mát hòa cùng ambroxan ấm nồng. Lưu hương lâu, hợp mọi hoàn cảnh.', N'Bergamot, Ambroxan, Vani', 2950000, 100, 25, N'https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Bleu de Chanel EDP', N'Chanel', N'Biểu tượng của sự thanh lịch: mở đầu tươi mát với cam và gừng, kết thúc bằng gỗ đàn hương sang trọng.', N'Cam, Gừng, Gỗ đàn hương', 3150000, 100, 20, N'https://images.unsplash.com/photo-1592945403244-b3fbafd7f539?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Versace Eros EDT', N'Versace', N'Hương bạc hà và táo xanh tươi mát quyện với vani ấm áp, thể hiện sự mạnh mẽ và đầy đam mê.', N'Bạc hà, Táo xanh, Vani', 1850000, 100, 30, N'https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Creed Aventus', N'Creed', N'Huyền thoại niche với dứa tươi, khói bạch dương và xạ hương, dành cho quý ông thành đạt.', N'Dứa, Bạch dương, Xạ hương', 8900000, 100, 8, N'https://images.unsplash.com/photo-1587017539504-67cfbddac569?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Dolce & Gabbana The One EDP', N'Dolce & Gabbana', N'Hương phương Đông ấm áp với thuốc lá, gừng và tuyết tùng, lịch lãm và quyến rũ.', N'Thuốc lá, Gừng, Tuyết tùng', 2250000, 100, 18, N'https://images.unsplash.com/photo-1615634260167-c8cdede054de?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Paco Rabanne 1 Million', N'Paco Rabanne', N'Hương bưởi, quế và da thuộc gợi cảm giác xa hoa, cá tính và bốc lửa.', N'Bưởi, Quế, Da thuộc', 2050000, 100, 22, N'https://images.unsplash.com/photo-1523293182086-7651a899d37f?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'YSL La Nuit de L''Homme', N'Yves Saint Laurent', N'Bạch đậu khấu quyện tuyết tùng và oải hương tạo nên sự nam tính bí ẩn cho buổi tối.', N'Bạch đậu khấu, Tuyết tùng, Oải hương', 2400000, 100, 16, N'https://images.unsplash.com/photo-1610461888750-10bfc601b874?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Acqua di Gio Profondo', N'Giorgio Armani', N'Hương biển sâu lắng với bergamot và khoáng chất, tươi mát và hiện đại.', N'Hương biển, Bergamot, Xạ hương', 2600000, 125, 20, N'https://images.unsplash.com/photo-1588405748880-12d1d2a59f75?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Allure Homme Sport', N'Chanel', N'Năng động và tinh tế: cam, tiêu và đậu tonka mang lại năng lượng tươi mới.', N'Cam, Tiêu, Đậu tonka', 2700000, 100, 15, N'https://images.unsplash.com/photo-1557170334-a9632e77c6e4?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Montblanc Explorer', N'Montblanc', N'Bergamot Ý, da thuộc và hoắc hương Indonesia cho quý ông ưa khám phá.', N'Bergamot, Da thuộc, Hoắc hương', 1500000, 100, 26, N'https://images.unsplash.com/photo-1616604426140-1cf14caf6b6a?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Hugo Boss Bottled', N'Hugo Boss', N'Táo, quế và gỗ tạo nên mùi hương của người đàn ông của công việc — chỉn chu, đáng tin.', N'Táo, Quế, Gỗ', 1650000, 100, 24, N'https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Prada L''Homme', N'Prada', N'Diên vĩ, phong lữ và tuyết tùng mang lại vẻ lịch lãm nhã nhặn, rất hợp công sở.', N'Diên vĩ, Phong lữ, Tuyết tùng', 2500000, 100, 14, N'https://images.unsplash.com/photo-1592945403244-b3fbafd7f539?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Tom Ford Oud Wood', N'Tom Ford', N'Trầm hương quý phái quyện gỗ hồng và bạch đậu khấu — sang trọng và khác biệt.', N'Trầm hương, Gỗ hồng, Bạch đậu khấu', 6500000, 100, 10, N'https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Tom Ford Tobacco Vanille', N'Tom Ford', N'Thuốc lá và vani ấm áp, ngọt ngào và ấm cúng cho mùa lạnh.', N'Thuốc lá, Vani, Gia vị', 6900000, 100, 9, N'https://images.unsplash.com/photo-1587017539504-67cfbddac569?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'JPG Le Male', N'Jean Paul Gaultier', N'Bạc hà, oải hương và vani — mùi hương biểu tượng vừa nam tính vừa ngọt ngào.', N'Bạc hà, Oải hương, Vani', 1900000, 125, 20, N'https://images.unsplash.com/photo-1615634260167-c8cdede054de?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Nautica Voyage', N'Nautica', N'Hương biển tươi mát với táo và gỗ, giá tốt — lựa chọn quen thuộc của sinh viên.', N'Táo, Hương biển, Gỗ', 450000, 100, 40, N'https://images.unsplash.com/photo-1523293182086-7651a899d37f?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Davidoff Cool Water', N'Davidoff', N'Bạc hà và oải hương mát lạnh cùng xạ hương biển — kinh điển, dễ dùng hằng ngày.', N'Bạc hà, Oải hương, Xạ hương biển', 700000, 125, 35, N'https://images.unsplash.com/photo-1610461888750-10bfc601b874?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Bvlgari Man In Black', N'Bvlgari', N'Rượu rum, hoa cam và da thuộc tạo nên nét đàn ông trưởng thành, quyến rũ.', N'Rượu rum, Hoa cam, Da thuộc', 2300000, 100, 15, N'https://images.unsplash.com/photo-1588405748880-12d1d2a59f75?auto=format&fit=crop&w=600&q=80', @buoiToi),
(N'Azzaro Wanted', N'Azzaro', N'Chanh, bạch đậu khấu và gỗ khô mang lại sự tươi mới, tự tin cho ngày dài.', N'Chanh, Bạch đậu khấu, Gỗ', 1700000, 100, 21, N'https://images.unsplash.com/photo-1557170334-a9632e77c6e4?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Gucci Guilty Pour Homme', N'Gucci', N'Oải hương, cam Ý và hoắc hương — hiện đại, phóng khoáng và cuốn hút.', N'Oải hương, Cam Ý, Hoắc hương', 2150000, 90, 17, N'https://images.unsplash.com/photo-1616604426140-1cf14caf6b6a?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Burberry Hero', N'Burberry', N'Bergamot, tuyết tùng và nhựa thơm gợi hình ảnh người đàn ông tự do, mạnh mẽ.', N'Bergamot, Tuyết tùng, Nhựa thơm', 2000000, 100, 19, N'https://images.unsplash.com/photo-1541643600914-78b084683601?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Calvin Klein CK One', N'Calvin Klein', N'Cam chanh, trà xanh và xạ hương — hương unisex nhẹ nhàng, trẻ trung.', N'Cam chanh, Trà xanh, Xạ hương', 850000, 100, 33, N'https://images.unsplash.com/photo-1592945403244-b3fbafd7f539?auto=format&fit=crop&w=600&q=80', @theThao),
(N'Montblanc Legend', N'Montblanc', N'Oải hương, táo và rêu sồi tạo nên nét lịch lãm cổ điển, đáng tin cậy.', N'Oải hương, Táo, Rêu sồi', 1600000, 100, 23, N'https://images.unsplash.com/photo-1594035910387-fea47794261f?auto=format&fit=crop&w=600&q=80', @congSo),
(N'Lacoste L.12.12 Blanc', N'Lacoste', N'Bưởi, cây bách và tuyết tùng — sạch sẽ, tươi mát, phù hợp mọi lứa tuổi.', N'Bưởi, Cây bách, Tuyết tùng', 1250000, 100, 28, N'https://images.unsplash.com/photo-1587017539504-67cfbddac569?auto=format&fit=crop&w=600&q=80', @theThao);

/* Chèn vào bảng Products nếu tên chưa tồn tại */
INSERT INTO dbo.Products (Name, Brand, [Description], Notes, Price, VolumeMl, Stock, ImageUrl, CreatedAt, CategoryId)
SELECT p.Name, p.Brand, p.Descr, p.Notes, p.Price, p.VolumeMl, p.Stock, p.ImageUrl, GETUTCDATE(), p.CategoryId
FROM @P p
WHERE NOT EXISTS (SELECT 1 FROM dbo.Products x WHERE x.Name = p.Name);

/* Cập nhật nốt hương cho 3 sản phẩm seed ban đầu (nếu đang trống) */
UPDATE dbo.Products SET Notes = N'Bergamot, Tiêu Sichuan'
  WHERE Name = N'Dior Sauvage EDT' AND (Notes IS NULL OR Notes = N'');
UPDATE dbo.Products SET Notes = N'Bạc hà, Vani'
  WHERE Name = N'Versace Eros EDT' AND (Notes IS NULL OR Notes = N'');
UPDATE dbo.Products SET Notes = N'Muối biển, Cam chanh'
  WHERE Name = N'Nautica Voyage' AND (Notes IS NULL OR Notes = N'');

PRINT N'Hoàn tất. Tổng số sản phẩm hiện có:';
SELECT COUNT(*) AS TongSanPham FROM dbo.Products;
GO
