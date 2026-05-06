USE db_TPS;
GO

DROP PROCEDURE IF EXISTS sp_Giaovien_Thongtin;
GO

CREATE PROCEDURE sp_Giaovien_Thongtin
	@GiaovienID DECIMAL(18,2) = NULL
AS 
BEGIN
	
	SELECT 
	gv.Hoten AS [Họ và tên], gv.Ngaysinh AS [Ngày sinh], gv.Gioitinh AS [Giới tính], gv.Email AS [Email], gv.Diachi AS [Địa chỉ], gv.Dienthoai AS [Số điện thoại],
	(
		SELECT cd.ChuyendeID AS ID, cd.Tenchuyende AS Ten
		FROM Giaovien_Chuyende gv_cd
		JOIN Chuyende cd ON gv_cd.ChuyendeID = cd.ChuyendeID
		WHERE gv_cd.GiaovienID = @GiaovienID AND gv_cd.Khongsudung = 0
		FOR JSON PATH, INCLUDE_NULL_VALUES
	) AS [Danh sách chuyên đề],
	(
		SELECT kh.KhoahocID AS ID, kh.Tenkhoahoc AS Ten
		FROM Giaovien_Khoahoc gv_kh
		JOIN Khoahoc kh ON gv_kh.KhoahocID = kh.KhoahocID
		WHERE gv_kh.GiaovienID = @GiaovienID AND gv_kh.Khongsudung = 0
		FOR JSON PATH, INCLUDE_NULL_VALUES
	) AS [Danh sach khoa hoc],
	(
		SELECT gv_cc.Tenchungchi AS [Ten chung chi], gv_cc.Donvicap AS [Don vi cap], gv_cc.Ngaycap AS [Ngay cap], gv_cc.Ngayhethan AS [Ngay het han]
		FROM Giaovien_Chungchi gv_cc
		WHERE gv_cc.GiaovienID = @GiaovienID AND gv_cc.Khongsudung = 0
		FOR JSON PATH, INCLUDE_NULL_VALUES
	) AS [Danh sach chung chi]
	FROM dbo.Giaovien gv
	WHERE gv.GiaovienID = @GiaovienID AND gv.Khongsudung = 0

END

EXEC sp_Giaovien_Thongtin @GiaovienID = 1

EXEC sp_Giaovien_Danhsach @Giaovien_Ten = N'Giáo viên 1', @Chuyende_Ten = N'Chuyên đề 1', @Khoahoc_Ten = N'Khóa học 1'