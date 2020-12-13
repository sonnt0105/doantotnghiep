CREATE DATABASE BATDONGSAN

-->Tài khoản Admin
CREATE TABLE Admin(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenTaiKhoan NVARCHAR(50),
	MatKhau NVARCHAR(50),
)

--> Tỉnh/Thành phố
CREATE TABLE TinhThanhPho (
  ID INT  IDENTITY(1,1) PRIMARY KEY,
  TenTinhThanhPho Nvarchar(50) 
)

--> Quận/Huyện
CREATE TABLE QuanHuyen (
  ID INT  IDENTITY(1,1) PRIMARY KEY,
  TenQuanHuyen Nvarchar(100) ,
  IDTinhThanhPho INT FOREIGN KEY REFERENCES  TinhThanhPho(ID),
) 

--> Phường/Xã
CREATE TABLE PhuongXa (
  ID INT  IDENTITY(1,1) PRIMARY KEY,
  TenPhuongXa NVARCHAR(50) ,
  IDQuanHuyen INT FOREIGN KEY REFERENCES  QuanHuyen(ID)
) 

--> Loại Bài Đăng
CREATE TABLE LoaiBaiDang(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenLoaiBaiDang NVARCHAR(80),
)

--> Loại nhà đất
CREATE TABLE LoaiNhaDat(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenLoaiNhaDat NVARCHAR(30),
	IDLoaiBaiDang INT FOREIGN KEY REFERENCES  LoaiBaiDang(ID)
)

--> Sales
CREATE TABLE Sales(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenSales NVARCHAR(50) not null,
	Email NVARCHAR(50) NOT NULL,
	SoDienThoai NVARCHAR(50) NOT NULL,
	DiaChi NVARCHAR(50) NOT NULL,
	TaiKhoan NVARCHAR(50) NOT NULL,
	MatKhau NVARCHAR(50) NOT NULL,
	TrangThai INT, 
	ChoKiemTra INT,
)

--> Nhân Viên
CREATE TABLE NhanVien(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	HoTen NVARCHAR(40) NOT NULL,
	TaiKhoan NVARCHAR (50) NOT NULL,
	MatKhau NVARCHAR (50) NOT NULL,
	Email NVARCHAR(40) NOT NULL,
	SoDienThoai NVARCHAR(20) NOT NULL,
	DiaChi NVARCHAR(100),
	TrangThai INT, 
	ChoKiemTra INT,
)

-->Loại dự án
CREATE TABLE LoaiDuAn(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenLoaiDuAn NVARCHAR(100),
)

--> Dự án
CREATE TABLE DuAn(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenDuAn NVARCHAR(100) not null,
	IDQuanHuyen INT FOREIGN KEY REFERENCES  QuanHuyen(ID) not null,
	IDPhuongXa INT FOREIGN KEY REFERENCES  PhuongXa(ID) ,
	IDTinhThanhPho INT FOREIGN KEY REFERENCES  TinhThanhPho(ID)not null,
	DiaChi NVARCHAR(200),
	DonViPhanPhoi NVARCHAR(100),
	TongDienTich FLOAT,
	GiaBan FLOAT,
	IDLoaiDuAn INT FOREIGN KEY REFERENCES  LoaiDuAn(ID) not null,
	MoTa NVARCHAR(3000),
	Urlimage NVARCHAR(max) null,
	TrangThai INT,
	NgayDang DATE not null,
	NgayCapNhat DATE not null,
	ChuDauTu NVARCHAR(100),
	QuyMoDuAn NVARCHAR(200),
	MatDoXayDung NVARCHAR(100),
	
)

--> Đơn vị giá
CREATE TABLE DonVi(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenDonVi NVARCHAR(20),
	IDLoaiBaiDang INT FOREIGN KEY REFERENCES  LoaiBaiDang(ID),
)

--> Bất Động sản
CREATE TABLE BatDongSan(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenBatDongSan NVARCHAR(100)not null,
	IDQuanHuyen INT FOREIGN KEY REFERENCES  QuanHuyen(ID)not null,
	IDPhuongXa INT FOREIGN KEY REFERENCES  PhuongXa(ID),
	IDTinhThanhPho INT FOREIGN KEY REFERENCES  TinhThanhPho(ID)not null,
	DiaChi NVARCHAR(200) NOT NULL,
	Gia FLOAT NOT NULL,
	IDDonVi INT FOREIGN KEY REFERENCES  DonVi(ID) not null,
	DienTich FLOAT NOT NULL,
	IDLoaiBaiDang INT FOREIGN KEY REFERENCES  LoaiBaiDang(ID) not null,	
	IDLoaiNhaDat INT FOREIGN KEY REFERENCES  LoaiNhaDat(ID) not null,	
	IDDuAn INT FOREIGN KEY REFERENCES  DuAn(ID),
	Mota NVARCHAR(3000),
	SoPhongNgu INT,
	SoTang NVARCHAR(20), 
	SoToilet NVARCHAR(1000),
	NoiThat NVARCHAR(1000),
	MatTien NVARCHAR(1000),
	HuongNha NVARCHAR(1000),
	Urlimage NVARCHAR(max) null,
	TrangThai INT,
	NgayDang DATE not null,
	NgayCapNhat DATE not null,
	TenLienHe NVARCHAR(50),
	DiaChiLienHe NVARCHAR(300),
	SoDienThoaiLienHe NVARCHAR(50),
	EmailLienHe NVARCHAR(50)
)


--> Phân công sales
CREATE TABLE PhanCong(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	IDBatDongSan INT FOREIGN KEY REFERENCES  BatDongSan(ID),
	IDSales INT FOREIGN KEY REFERENCES  Sales(ID),
	PhanTramHoaHong FLOAT,
	NgayTao DATE
)

--> Phí Hoa Hồng
CREATE TABLE PhiHoaHong(
	ID INT  IDENTITY(1,1) PRIMARY KEY,	
	TongTien FLOAT,
	IDSales INT FOREIGN KEY REFERENCES  Sales(ID),
	IDPhanCong INT FOREIGN KEY REFERENCES  PhanCong(ID),
	NgayTao DATE,
	NgayBan DATE
)

--> bảng điểm
CREATE TABLE DIEM(
	ID INT  IDENTITY(1,1) PRIMARY KEY,
	TenBaiThi NVARCHAR(100),
	SoDiem FLOAT NOT NULL,
	NgayThi DATE NOT NULL,
	IDNhanVien INT FOREIGN KEY REFERENCES  NhanVien(ID),
	IDSale INT FOREIGN KEY REFERENCES  Sales(ID),
)

--> Bảng câu hỏi
CREATE TABLE CauHoi(
	ID INT PRIMARY KEY,
	NoiDung NVARCHAR(500),
	DotThi INT,	
)

--> Bảng câu trả lời
CREATE TABLE TraLoi(
	ID INT PRIMARY KEY,
	IDCauHoi INT FOREIGN KEY REFERENCES  CauHoi(ID),	
	DapAn NVARCHAR(500),
	DungSai INT,
)



INSERT INTO Admin(tentaikhoan,matkhau) VALUES ('admin','123')

INSERT INTO CauHoi([ID],[DotThi]) values (1,0)


-->lấy dữ liệu vào bảng tỉnh thành phố
SET IDENTITY_INSERT [dbo].[TinhThanhPho] ON 
INSERT INTO TinhThanhPho(ID,TenTinhThanhPho)
SELECT [Id],[TenTinh] FROM [dbo].[DM_Tinh]
SET IDENTITY_INSERT [dbo].[TinhThanhPho] OFF 

-->lấy dữ liệu vào bảng quận huyện 
SET IDENTITY_INSERT [dbo].[QuanHuyen] ON 
INSERT INTO QuanHuyen(ID,TenQuanHuyen,IDTinhThanhPho) 
SELECT DM_Huyen.Id,TenHuyen,TinhThanhPho.ID FROM [dbo].[DM_Huyen] JOIN  [dbo].[TinhThanhPho] ON DM_Huyen.IDTinh = TinhThanhPho.ID
SET IDENTITY_INSERT [dbo].[QuanHuyen] OFF 

--> lấy dữ liệu vào bảng phường xã
SET IDENTITY_INSERT [dbo].[PhuongXa] ON 
INSERT INTO PhuongXa(ID,TenPhuongXa,IDQuanHuyen) 
SELECT DM_PhuongXa.Id,TenPhuongXa,QuanHuyen.ID FROM  [dbo].[DM_PhuongXa] JOIN [dbo].[QuanHuyen]  ON DM_PhuongXa.IDHuyen = QuanHuyen.ID
SET IDENTITY_INSERT [dbo].[PhuongXa] OFF 


--> INSERT Loại dự án
INSERT INTO LoaiDuAn(TenLoaiDuAn) VALUES
( N'Căn hộ, Chung cư'),
( N'Cao ốc, văn phòng'),
( N'Trung tâm thương mại'),
( N'Khu đô thị mới'),
( N'Khu phức hợp'),
( N'Khu nghỉ dưỡng, sinh thái'),
( N'Khu công nghiệp'),
( N'Biệt thự, liền kề'),
( N'Nhà ở xã hội'),
( N'Dự án khác')

SET IDENTITY_INSERT DuAn ON 
-- tạo cái này trước khi tạo cái dưới
INSERT INTO DuAn([ID],[TenDuAn],[IDQuanHuyen],[IDPhuongXa],[IDTinhThanhPho],[IDLoaiDuAn],[NgayDang],[NgayCapNhat]) VALUES
(1,N'Dự án khác',1,1,1,10,'1/1/2000','1/1/2000')
SET IDENTITY_INSERT DuAn OFF 




--> Insert Loại Bài đăng
insert into LoaiBaiDang(tenloaibaidang) values
( N'Nhà đất bán'),
( N'Nhà đất cho thuê')


--> insert Lại nhà đất
INSERT INTO LoaiNhaDat(TenLoaiNhaDat,IDLoaiBaiDang) VALUES
(N'Bán căn hộ chung cư',1),
(N'Bán nhà riêng',1),
(N'Bán biệt thự, liền kề',1),
(N'Bán nhà mặt phố',1),
(N'Bán đất nền dự án',1),
(N'Bán đất',1),
(N'Trang trạ, khu nghỉ dưỡng',1),
(N'Kho, nhà xưởng',1),
(N'Bất động sản khác',1),
(N'Căn hộ chung cư',2),
(N'Nhà riêng',2),
(N'Nhà mặt phố',2),
(N'Nhà trọ, Phòng trọ',2),
(N'Văn phòng',2),
(N'Cửa hàng, ki ốt',2),
(N'Khi, nhà xưởng, đất',2),
(N'Bất động sản khác',2)

--> Đơn vị giá 
INSERT INTO DonVi(TenDonVi,IDLoaiBaiDang) VALUES
(N'Triệu',1),
(N'Tỷ',1),
(N'Trăm nghìn/m2',1),
(N'Triệu/m2',1),
(N'Trăm nghìn/tháng',2),
(N'Triệu/tháng',2),
(N'Nghìn/m2/tháng',2),
(N'Trăm nghìn/m2/tháng',2),
(N'Triệu/m2/tháng',2)

--> Insert Nhân Viên
insert into NhanVien(hoten,taikhoan,matkhau,email, sodienthoai, diachi,TrangThai,ChoKiemTra) values
( N'Nguyễn Thanh Sơn',N'nhanvien1',N'123', N'thanhson@gmail.com', N'012416492', N'12, Nguyễn Văn Bảo, Phường 4, Gò Vấp, TP.Hồ Chí Minh',1,0),
( N'Đinh Khánh Vỹ',N'nhanvien2',N'123', N'khanhvy@gmail.com', N'0947506825', N'12, 98/7, Ấp 6, Xuân Thới Sơn, Hóc Môn, TP.Hồ Chí Minh',1,0),
( N'Bùi Thị Hoài Thương',N'nhanvien3',N'123', N'hoaithuong@gmail.com',N'0924492823', N'38 Cây keo, Tam Phú, Thủ Đức, TP.Hồ Chí Minh',1,0)

--> insert Sales
INSERT INTO Sales(TenSales,Email,SoDienThoai,DiaChi,TaiKhoan,MatKhau,TrangThai,ChoKiemTra) values
(N'Nguyễn Chí Tâm',N'chitam@gmail.com',N'0339115632',N'8A đường 59,Gò Vấp',N'Sale1','123',1,0),
(N'Đặng Phát Tài',N'phattai@gmail.com',N'0339115633',N'8A đường 59,Gò Vấp',N'Sale2','123',1,0),
(N'Phạm Hùng Quân',N'hungquan@gmail.com',N'0339115634',N'8A đường 59,Gò Vấp',N'Sale3','123',1,0)




----------------------------------TEST---------------------------------------

select

SELECT idtinhthanhpho from TinhThanhPho
SELECT TOP 1 idtinhthanhpho FROM TinhThanhPho  
ORDER BY rand()  

insert into NhanVien(hoten,email, sodienthoai, diachi, phongban) values
( N'Lê Nhật Văn', N'nhatvan@gmail.com', N'012416492', N'12, Nguyễn Văn Bảo, Phường 4, Gò Vấp, TP.Hồ Chí Minh',
 (SELECT TOP 1 idphongban FROM PhongBan  ORDER BY NEWID()))



 --> QUICK INSERT DATA
 INSERT INTO TinhThanhPho(tentinhthanhpho) SELECT id,tentinhthanhpho FROM TinhThanhPho

--> delete all data in table Baidang
DELETE FROM Baidang;

DELETE FROM Baidang WHERE idbaidang=9;

DELETE FROM QuanHuyen WHERE ID=1;

DELETE FROM TinhThanhPho WHERE ID = 20;
