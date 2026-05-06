USE db_TPS;
GO

PRINT N'Bắt đầu quá trình sinh dữ liệu mẫu...';

-- Sử dụng Transaction để đảm bảo tính toàn vẹn và tăng tốc độ Insert hàng loạt
BEGIN TRAN;

DECLARE @i INT = 1;
DECLARE @TongSoDong INT = 60; -- Sinh 60 dòng cho mỗi bảng (Thỏa mãn yêu cầu ít nhất 50 dòng)

-- Chèn dữ liệu theo thứ tự từ Bảng Cha (Không có FK) đến Bảng Con (Có FK)
WHILE @i <= @TongSoDong
BEGIN
    -- =======================================================
    -- KHỐI 1: CÁC BẢNG DANH MỤC ĐỘC LẬP (Không chứa khóa ngoại)
    -- =======================================================
    
    -- 1. Hocvien
    INSERT INTO Hocvien (HocvienID, Hoten, Ngaysinh, Gioitinh, Email, Diachi, Dienthoai, Khongsudung)
    VALUES (@i, N'Học viên ' + CAST(@i AS NVARCHAR), DATEADD(YEAR, -20, GETDATE()), N'Nam', 'hocvien' + CAST(@i AS VARCHAR) + '@gmail.com', N'Địa chỉ học viên ' + CAST(@i AS NVARCHAR), '090' + RIGHT('0000000' + CAST(@i AS VARCHAR), 7), 0);

    -- 2. Giaovien
    INSERT INTO Giaovien (GiaovienID, Hoten, Ngaysinh, Gioitinh, Email, Diachi, Dienthoai, Khongsudung)
    VALUES (@i, N'Giáo viên ' + CAST(@i AS NVARCHAR), DATEADD(YEAR, -35, GETDATE()), N'Nữ', 'giaovien' + CAST(@i AS VARCHAR) + '@gmail.com', N'Địa chỉ giáo viên ' + CAST(@i AS NVARCHAR), '091' + RIGHT('0000000' + CAST(@i AS VARCHAR), 7), 0);

    -- 3. Khoahoc
    INSERT INTO Khoahoc (KhoahocID, Tenkhoahoc, Mota, Diemdat, Khongsudung)
    VALUES (@i, N'Khóa học ' + CAST(@i AS NVARCHAR), N'Mô tả chi tiết khóa học ' + CAST(@i AS NVARCHAR), 8.00, 0);

    -- 4. Chuyende
    INSERT INTO Chuyende (ChuyendeID, Tenchuyende, Mota, URL, Trangthai, Khongsudung)
    VALUES (@i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), N'Mô tả chuyên đề ' + CAST(@i AS NVARCHAR), 'http://url-anh-chuyen-de/' + CAST(@i AS VARCHAR) + '.jpg', N'Đang hoạt động', 0);


    -- =======================================================
    -- KHỐI 2: CÁC BẢNG QUAN HỆ CẤP 1
    -- =======================================================

    -- 5. Giaovien_Chungchi
    INSERT INTO Giaovien_Chungchi (MaID, GiaovienID, Giaovien_Ten, Tenchungchi, Mota, Donvicap, Ngaycap, Ngayhethan, Khongsudung)
    VALUES (@i, @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), N'Chứng chỉ sư phạm ' + CAST(@i AS NVARCHAR), N'Mô tả chứng chỉ GV', N'Bộ Giáo Dục', DATEADD(YEAR, -2, GETDATE()), NULL, 0);

    -- 6. Cauhinhchungchi
    INSERT INTO Cauhinhchungchi (CauhinhID, KhoahocID, Khoahoc_Ten, Tenchungchi, Mota, Donvicap, Ngaycap, Thoigiansudung, Ngayhethan, Khongsudung)
    VALUES (@i, @i, N'Khóa học ' + CAST(@i AS NVARCHAR), N'Chứng chỉ hoàn thành KH ' + CAST(@i AS NVARCHAR), N'Mô tả cấu hình CC', N'Trung tâm TPS', GETDATE(), 24, DATEADD(YEAR, 2, GETDATE()), 0);

    -- 7. Hocvien_Khoahoc
    INSERT INTO Hocvien_Khoahoc (MaID, HocvienID, Hocvien_Ten, KhoahocID, Khoahoc_Ten, Diemso, Trangthai, Khongsudung)
    VALUES (@i, @i, N'Học viên ' + CAST(@i AS NVARCHAR), @i, N'Khóa học ' + CAST(@i AS NVARCHAR), 8.5, N'Đang học', 0);

    -- 8. Giaovien_Khoahoc
    INSERT INTO Giaovien_Khoahoc (MaID, GiaovienID, Giaovien_Ten, KhoahocID, Khoahoc_Ten, Khongsudung)
    VALUES (@i, @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), @i, N'Khóa học ' + CAST(@i AS NVARCHAR), 0);

    -- 9. Giaovien_Chuyende
    INSERT INTO Giaovien_Chuyende (MaID, GiaovienID, Giaovien_Ten, ChuyendeID, Chuyende_Ten, Khongsudung)
    VALUES (@i, @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), 0);

    -- 10. Tailieu
    INSERT INTO Tailieu (TailieuID, ChuyendeID, Chuyende_Ten, Tieude, Ngaytao, Loaitailieu, Kichthuoc, Trangthai, Khongsudung)
    VALUES (@i, @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), N'Tài liệu PDF bài ' + CAST(@i AS NVARCHAR), GETDATE(), 'pdf', 5.5, N'Đang hoạt động', 0);

    -- 11. Cauhoi
    INSERT INTO Cauhoi (CauhoiID, ChuyendeID, Chuyende_Ten, Tencauhoi, Loaicauhoi, Dokho, Tongdiem, Trang_thai, Khongsudung)
    VALUES (@i, @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), N'Câu hỏi trắc nghiệm số ' + CAST(@i AS NVARCHAR), N'Trắc nghiệm', N'Dễ', 1.0, N'Hoạt động', 0);

    -- 12. Lichhoc
    INSERT INTO Lichhoc (LichhocID, KhoahocID, Khoahoc_Ten, ChuyendeID, Chuyende_Ten, GiaovienID, Giaovien_Ten, Thu, Ngaybatdau, Ngaykethuc, Thoidiembatdau, Thoigianhoc, Trangthai, Khonghoatdong)
    VALUES (@i, @i, N'Khóa học ' + CAST(@i AS NVARCHAR), @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), N'thu_2', GETDATE(), DATEADD(MONTH, 2, GETDATE()), '08:00:00', 120, N'Hoạt động', 0);

    -- 13. Cauhinhbaithuhoach
    INSERT INTO Cauhinhbaithuhoach (MaID, KhoahocID, Khoahoc_Ten, Tieude, Mota, Thoigianlambai, Diemdat, Trangthai, Tongsocauhoi, Khongsudung)
    VALUES (@i, @i, N'Khóa học ' + CAST(@i AS NVARCHAR), N'Bài thu hoạch số ' + CAST(@i AS NVARCHAR), N'Yêu cầu sinh viên hoàn thành...', 60.0, 5.0, N'Hoạt động', 10, 0);

    -- 14. Cathuchanh
    INSERT INTO Cathuchanh (CathuchanhID, KhoahocID, Khoahoc_Ten, ChuyendeID, Chuyende_Ten, GiaovienID, Giaovien_Ten, Tenca, Mota, Ngaythuchanh, Thoidiembatdau, Thoigianthuchanh, PhongID, Soluonghocvien, Soluongdangky, Trangthai)
    VALUES (@i, @i, N'Khóa học ' + CAST(@i AS NVARCHAR), @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), N'Ca thực hành sáng ' + CAST(@i AS NVARCHAR), N'Mô tả thực hành', GETDATE(), '07:30:00', 150.0, N'P_A' + CAST(@i AS NVARCHAR), 40, 25, N'Hoạt động');


    -- =======================================================
    -- KHỐI 3: CÁC BẢNG QUAN HỆ CẤP 2 (Phụ thuộc vào cấp 1)
    -- =======================================================

    -- 15. Cauhoi_Dapan (Phụ thuộc Cauhoi)
    INSERT INTO Cauhoi_Dapan (Dapan_ID, CauhoiID, Cauhoi_Ten, Noidung, Dung_sai, Trangthai)
    VALUES (@i, @i, N'Câu hỏi trắc nghiệm số ' + CAST(@i AS NVARCHAR), N'Đáp án chính xác của câu ' + CAST(@i AS NVARCHAR), 1, N'Hoạt động');

    -- 16. Chitiecauhinhbaithuhoach (Phụ thuộc Cauhinhbaithuhoach)
    INSERT INTO Chitiecauhinhbaithuhoach (MaID, CauhinhID, Cauhinh_Mota, ChuyendeID, Chuyende_Ten, Dokho, Soluong, Khongsudung)
    VALUES (@i, @i, N'Yêu cầu sinh viên hoàn thành...', @i, N'Chuyên đề ' + CAST(@i AS NVARCHAR), N'Dễ', 5, 0);

    -- 17. Baithi (Phụ thuộc Cauhinhbaithuhoach, Hocvien, Khoahoc)
    INSERT INTO Baithi (BaithiID, HocvienID, Hocvien_Ten, KhoahocID, Khoahoc_Ten, CauhinhID, Cauhinh_Mota, Cauhinh_Thoigianlambai, Cauhinh_Diemdat, Cauhinh_Tieude, Thoigianlambai, Thoidiembatdau, Diemthi, Tongsocauhoi, Socaudung, Socausai, Socaukhongtraloi, Trangthai, Khongsudung)
    VALUES (@i, @i, N'Học viên ' + CAST(@i AS NVARCHAR), @i, N'Khóa học ' + CAST(@i AS NVARCHAR), @i, N'Yêu cầu...', 60.0, 5.0, N'Bài thu hoạch số ' + CAST(@i AS NVARCHAR), 45.0, GETDATE(), 8.0, 10, 8, 2, 0, N'Đã hoàn thành', 0);

    -- 18. Cathuchanh_Dangky (Phụ thuộc Cathuchanh, Hocvien)
    INSERT INTO Cathuchanh_Dangky (MaID, HocvienID, Hocvien_Ten, CathuchanhID, Cathuchanh_Ten, Trangthai)
    VALUES (@i, @i, N'Học viên ' + CAST(@i AS NVARCHAR), @i, N'Ca thực hành sáng ' + CAST(@i AS NVARCHAR), N'Đăng ký');

    -- 19. Chungchi (Phụ thuộc Cauhinhchungchi, Khoahoc, Hocvien)
    INSERT INTO Chungchi (ChungchiID, HocvienID, Hocvien_Ten, KhoahocID, Khoahoc_Ten, CauhinhchungchiID, Cauhinhchungchi_Tenchungchi, Cauhinhchungchi_Mota, Cauhinhchungchi_Donvicap, Ngaycap, Cauhinhchungchi_Thoigiansudung, Ngayhethan, Khongsudung)
    VALUES (@i, @i, N'Học viên ' + CAST(@i AS NVARCHAR), @i, N'Khóa học ' + CAST(@i AS NVARCHAR), @i, N'Chứng chỉ hoàn thành KH ' + CAST(@i AS NVARCHAR), N'Mô tả', N'Trung tâm TPS', GETDATE(), 24, DATEADD(YEAR, 2, GETDATE()), 0);

    -- 20. Diemdanh (Phụ thuộc Giaovien, Hocvien, Khoahoc)
    INSERT INTO Diemdanh (DiemdanhID, GiaovienID, Giaovien_Ten, HocvienID, Hocvien_Ten, KhoahocID, Khoahoc_Ten, Thoidiemdiemdanh, trang_thai, Khongsudung)
    VALUES (@i, @i, N'Giáo viên ' + CAST(@i AS NVARCHAR), @i, N'Học viên ' + CAST(@i AS NVARCHAR), @i, N'Khóa học ' + CAST(@i AS NVARCHAR), GETDATE(), N'đã điểm danh', 0);


    -- =======================================================
    -- KHỐI 4: CÁC BẢNG QUAN HỆ CẤP 3 (Phụ thuộc sâu nhất)
    -- =======================================================

    -- 21. Baithi_Cauhoi (Phụ thuộc Baithi, Cauhoi)
    INSERT INTO Baithi_Cauhoi (MaID, BaithiID, Baithi_Tieude, CauhoiID, Cauhoi_Ten, Tongdiem)
    VALUES (@i, @i, N'Bài thu hoạch số ' + CAST(@i AS NVARCHAR), @i, N'Câu hỏi trắc nghiệm số ' + CAST(@i AS NVARCHAR), 1.0);

    -- 22. Baithi_Cauhoi_Traloi (Phụ thuộc Baithi_Cauhoi, Cauhoi_Dapan)
    INSERT INTO Baithi_Cauhoi_Traloi (MaID, Baithi_CauhoiID, Baithi_Tieude, Cauhoi_DapanID, Cauhoi_Dapan_Noidung, Dung_sai)
    VALUES (@i, @i, N'Bài thu hoạch số ' + CAST(@i AS NVARCHAR), @i, N'Đáp án chính xác của câu ' + CAST(@i AS NVARCHAR), 1);

    -- Tăng biến đếm lên 1 để chạy dòng tiếp theo
    SET @i = @i + 1;
END

COMMIT TRAN;

PRINT N'==========================================================';
PRINT N'Đã hoàn tất sinh dữ liệu. 60 dòng đã được chèn vào mỗi bảng.';
PRINT N'==========================================================';
GO