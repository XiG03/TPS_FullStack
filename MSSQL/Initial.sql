
CREATE DATABASE db_TPS;
GO
USE db_TPS;
GO



CREATE TABLE Hocvien(
	HocvienID DECIMAL(18,2) PRIMARY KEY,
	Hoten NVARCHAR(200),
	Ngaysinh DATETIME,
	Gioitinh NVARCHAR(10), -- Nam, Nữ, Khác
	Email VARCHAR(50),
	Diachi NVARCHAR(200),
	Dienthoai NVARCHAR(20),
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Giaovien(
	GiaovienID DECIMAL(18,2) PRIMARY KEY,
	Hoten NVARCHAR(200),
	Ngaysinh DATETIME,
	Gioitinh NVARCHAR(10), -- Nam, Nữ, Khác
	Email VARCHAR(50) ,
	Diachi NVARCHAR(250),
	Dienthoai NVARCHAR(20),
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Giaovien_Chungchi(
	MaID DECIMAL(18,2) PRIMARY KEY,
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(200),
	Tenchungchi NVARCHAR(200),
	Mota NVARCHAR(200),
	Donvicap NVARCHAR(200),
	Ngaycap DATETIME,
	Ngayhethan DATETIME, -- Ngày hết hạn NULL => Chứng chỉ vô thời hạn
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);


CREATE TABLE Khoahoc(
	KhoahocID DECIMAL(18,2) PRIMARY KEY,
	Tenkhoahoc NVARCHAR(100),
	Mota NVARCHAR(500),
	Diemdat DECIMAL(5,2) DEFAULT 8.000,-- Điểm đậu của khóa học, mặc định là 8.000
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Cauhinhchungchi( -- Cấu hình chứng chỉ cho khóa học nếu pass
	CauhinhID DECIMAL(18,2) PRIMARY KEY,
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	Tenchungchi NVARCHAR(200),
	Mota NVARCHAR(500),
	Donvicap NVARCHAR(200),
	Ngaycap DATETIME, -- Ngày cấp chứng chỉ
	Thoigiansudung DECIMAL(18,2) NULL, -- Thời gian sử dụng tính bằng tháng, NULL nếu vô thời hạn
	Ngayhethan DATETIME, -- Ngày hết hạn NULL => Chứng chỉ vô thời hạn
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng

);

CREATE TABLE Hocvien_Khoahoc(
	MaID DECIMAL(18,2) PRIMARY KEY,
	HocvienID DECIMAL(18,2),
	Hocvien_Ten NVARCHAR(200),
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	Diemso DECIMAL(5,2), -- Điểm số hiện tại của học viên trong khóa học, NULL nếu chưa có điểm
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Đang học', -- Trạng thái: Đang học, Đã hoàn thành, Bị khóa
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Giaovien_Khoahoc(
	MaID DECIMAL(18,2) PRIMARY KEY,
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(100),
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Chuyende(
	ChuyendeID DECIMAL(18,2) PRIMARY KEY,
	Tenchuyende NVARCHAR(50),
	Mota NVARCHAR(500),
	URL VARCHAR(255), -- Duong dan anh dai dien cho chuyên đề
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Đang hoạt động', -- Trạng thái: Đang hoạt động, Ngừng hoạt động
	meta_data NVARCHAR(MAX) NULL,
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Giaovien_Chuyende(
	MaID DECIMAL(18,2) PRIMARY KEY,
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(200),
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	Khongsudung BIT NULL, -- 0: đang sử dụng, 1: không sử dụng
);


-- Critical
CREATE TABLE Tailieu(
	TailieuID DECIMAL(18,2) PRIMARY KEY,
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	Tieude NVARCHAR(200), -- File_name
	Ngaytao DATETIME DEFAULT GETDATE(), -- File_date
	Loaitailieu NVARCHAR(50), -- File_type: video, pdf, docx, v.v.
	Kichthuoc DECIMAL(18,2), -- File_size tính bằng MB
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Đang hoạt động', -- Trạng thái: Đang hoạt động, Ngừng hoạt động
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);


CREATE TABLE Cauhoi( 
	CauhoiID DECIMAL(18,2) PRIMARY KEY,
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	Tencauhoi NVARCHAR(200),
	Loaicauhoi NVARCHAR(50) NOT NULL, -- Loại câu hỏi: Trắc nghiệm, Đúng sai, Multiple choice
	Dokho NVARCHAR(50) NOT NULL DEFAULT 'Dễ', -- Độ khó: Dễ, Trung bình, Khó
	Tongdiem DECIMAL(5,2), -- Tổng điểm của câu hỏi (với trường hợp trắc nghiệm: 1, Multiple choice: N)
	Trang_thai NVARCHAR(20) NOT NULL DEFAULT 'Hoạt động', -- Trạng thái: Hoạt động, Ngừng hoạt động
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Cauhoi_Dapan(
	Dapan_ID DECIMAL(18,2) PRIMARY KEY,
	CauhoiID DECIMAL(18,2),
	Cauhoi_Ten NVARCHAR(200),
	Noidung NVARCHAR(200),
	Dung_sai BIT , -- 0: sai, 1: đúng
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Hoạt động', -- Trạng thái: Hoạt động, Ngừng hoạt động
);

CREATE TABLE Lichhoc(
	LichhocID DECIMAL(18,2) PRIMARY KEY,
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(200),
	Thu NVARCHAR(20), -- Thứ học: thu_2, thu_3, ..., thu_7
	Ngaybatdau DATETIME DEFAULT GETDATE(),
	Ngaykethuc DATE,
	Thoidiembatdau TIME, -- Thời điểm bắt đầu học
	Thoigianhoc DECIMAL(4,1), -- Thời gian học tính bằng phút
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Hoạt động', -- Trạng thái: Hoat động, Đã kết thúc, Tạm dừng
	Khonghoatdong BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Cauhinhbaithuhoach(
	MaID DECIMAL(18,2) PRIMARY KEY,
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	Tieude NVARCHAR(200),
	Mota NVARCHAR(MAX),
	Thoigianlambai DECIMAL (4,1), -- Thời gian làm bài tính bằng phút
	Diemdat DECIMAL(5,2), -- Điểm vượt qua để đậu bài thi
	Trangthai NVARCHAR(20) DEFAULT 'Hoạt động', -- Trạng thái: Hoạt động, ngưng hoạt động
	Tongsocauhoi DECIMAL (3,1), -- Tổng số câu hỏi trong bài thi
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);

CREATE TABLE Chitiecauhinhbaithuhoach(
	MaID DECIMAL(18,2) PRIMARY KEY,
	CauhinhID DECIMAL(18,2),
	Cauhinh_Mota NVARCHAR(MAX),
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	Dokho NVARCHAR(50), -- Độ khó: Dễ, Trung bình, Khó
	Soluong DECIMAL(18,2), -- Số lượng câu hỏi của độ khó này trong bài thi
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
);

-- Critical 
CREATE TABLE Baithi(
	BaithiID DECIMAL(18,2) PRIMARY KEY,
	HocvienID DECIMAL(18,2),
	Hocvien_Ten NVARCHAR(200),
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	CauhinhID DECIMAL(18,2),
	Cauhinh_Mota NVARCHAR(MAX),
	Cauhinh_Thoigianlambai DECIMAL (4,1), -- Thời gian làm bài theo cấu hình tính bằng phút
	Cauhinh_Diemdat DECIMAL(5,2), -- Điểm đậu theo cấu hình
	Cauhinh_Tieude NVARCHAR(200),
	Thoigianlambai DECIMAL (4,1), -- Thời gian làm bài thực tế tính bằng phút, nếu đang thực hiện, tính NULL
	Thoidiembatdau DATETIME DEFAULT GETDATE(),
	Diemthi DECIMAL(5,2) NULL, -- Điểm thi của học viên, NULL nếu chưa thi
	Tongsocauhoi INT NOT NULL, -- Tổng số câu hỏi trong bài thi
	Socaudung INT NULL, -- Số câu đúng của học viên, NULL nếu chưa thi
	Socausai INT NULL, -- Số câu sai của học viên, NULL nếu chưa thi, đối với câu hỏi trả lời null -> tính là sai
	Socaukhongtraloi INT NULL, -- Số câu không trả lời của học viên, NULL nếu chưa thi
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Đang làm bài', -- Trạng thái: Đang làm bài, Đã hoàn thành, Bị hủy
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
	
);

CREATE TABLE Baithi_Cauhoi(
	MaID DECIMAL(18,2) PRIMARY KEY,
	BaithiID DECIMAL(18,2),
	Baithi_Tieude NVARCHAR(200),
	CauhoiID DECIMAL(18,2),
	Cauhoi_Ten NVARCHAR(200),
	Tongdiem DECIMAL(5,2), -- Điểm thi của câu hỏi, NULL nếu chưa chấm (điểm thi này được quy vào trường hợp câu hỏi có nhiều lựa chọn)
	
);

CREATE TABLE Baithi_Cauhoi_Traloi(
	MaID DECIMAL(18,2) PRIMARY KEY,
	Baithi_CauhoiID DECIMAL(18,2),
	Baithi_Tieude NVARCHAR(200),
	Cauhoi_DapanID DECIMAL(18,2),
	Cauhoi_Dapan_Noidung NVARCHAR(200), -- Nội dung đáp án mà học viên đã chọn, NULL nếu chưa trả lời
	Dung_sai BIT NULL -- 0: sai, 1: đúng, NULL nếu chưa trả lời
);


CREATE TABLE Cathuchanh(
	CathuchanhID DECIMAL(18,2) PRIMARY KEY,
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	ChuyendeID DECIMAL(18,2),
	Chuyende_Ten NVARCHAR(50),
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(200),
	Tenca NVARCHAR(50),
	Mota NVARCHAR(MAX),
	Ngaythuchanh DATETIME,
	Thoidiembatdau TIME,
	Thoigianthuchanh DECIMAL(4,1), -- Thời gian thực hành tính bằng phút
	PhongID NVARCHAR(50) NULL, -- Phòng thực hành, có thể là tên phòng hoặc mã phòng
	Soluonghocvien DECIMAL(4,1) NULL, -- Số lượng tối đa học viên tham gia thực hành 
	Soluongdangky DECIMAL(4,1) NULL, -- Số lượng học viên đã đăng ký thực hành
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Hoạt động', -- Trạng thái: Hoạt động, Ngưng hoạt động
);

CREATE TABLE Cathuchanh_Dangky(
	MaID DECIMAL(18,2) PRIMARY KEY,
	HocvienID DECIMAL(18,2),
	Hocvien_Ten NVARCHAR(200),
	CathuchanhID DECIMAL(18,2),
	Cathuchanh_Ten NVARCHAR(50),
	Trangthai NVARCHAR(20) NOT NULL DEFAULT 'Đăng ký', -- Trạng thái: Đăng ký, Đã hủy
);

CREATE TABLE Chungchi( -- Bảng này sẽ record lại thông tin toàn bộ chứng chỉ của học viên sau khi hoàn thành khóa học và bài thi
	ChungchiID DECIMAL(18,2) PRIMARY KEY,
	HocvienID DECIMAL(18,2),
	Hocvien_Ten NVARCHAR(200),
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	CauhinhchungchiID DECIMAL(18,2),
	Cauhinhchungchi_Tenchungchi NVARCHAR(200),
	Cauhinhchungchi_Mota NVARCHAR(500),
	Cauhinhchungchi_Donvicap NVARCHAR(255),
	Ngaycap DATETIME,
	Cauhinhchungchi_Thoigiansudung DECIMAL(18,2) NULL, -- Thời gian sử dụng tính bằng tháng, NULL nếu vô thời hạn
	Ngayhethan DATETIME, -- Ngày hết hạn NULL => Chứng chỉ vô thời hạn
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
)

CREATE TABLE Diemdanh(
	DiemdanhID DECIMAL(18,2) PRIMARY KEY,
	GiaovienID DECIMAL(18,2),
	Giaovien_Ten NVARCHAR(200),
	HocvienID DECIMAL(18,2),
	Hocvien_Ten NVARCHAR(200),
	KhoahocID DECIMAL(18,2),
	Khoahoc_Ten NVARCHAR(100),
	Thoidiemdiemdanh DATETIME,
	trang_thai NVARCHAR(50) DEFAULT 'vang', -- Trạng thái: đã điểm danh, chờ duyệt, vắng
	Khongsudung BIT NULL -- 0: đang sử dụng, 1: không sử dụng
)




