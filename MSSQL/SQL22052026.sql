use TPS_db

ALTER TABLE dbo.Chuyende_Cauhoi
DROP COLUMN Diem


DELETE dbo.Chuyende_Cauhoi
WHERE MaID = @MaID OR ChuyendeID = @ChuyendeID


INSERT INTO dbo.Chuyende_Cauhoi(MaID, ChuyendeID, Ten)
VALUES (@MaID, @ChuyendeID, @Ten)


SELECT * FROM dbo.Chuyende_Cauhoi;

SELECT * FROM dbo.Chuyende_Cauhoi
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)



UPDATE dbo.Chuyende_Cauhoi
    SET
        Ten = @Ten
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)




INSERT INTO dbo.Chuyende_Dapan (MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung)
VALUE (@MaID, @ChuyendeID, @Chuyende_CauhoiID, @Ten, @Dung)

UPDATE dbo.Chuyende_Dapan
SET
    Ten = @Ten
    Dung = @Dung
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)




DELETE dbo.Chuyende_Dapan
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)


SELECT MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung
FROM dbo.Chuyende_Dapan


SELECT MaID, ChuyendeID, Chuyende_CauhoiID, Ten, Dung FROM dbo.Chuyende_Dapan
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)

INSERT INTO dbo.Chuyende_Tailieu (MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc)
VALUES (@MaID, @ChuyendeID, @Tieude, @Ngaytao, @Loaitailileu, @Kichthuoc)


SELECT MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc
FROM dbo.Chuyende_Tailieu


use TPS_db

INSERT INTO dbo.Chuyende(MaID, Ten, Mota, Khongsudung, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy)
VALUES (@MaID, Ten, @Mota, @Khongsudung, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy,  @DeletedAt, @DeletedBy)




UPDATE dbo.Chuyende
SET
    DeletedAt = @DeletedAt
    DeletedBy = @DeletedBy
WHERE MaID = @MaID


SELECT MaID, Ten, Mota, Khongsudung
FROM dbo.Chuyende
WHERE MaID = @MaID AND Khongsudung = 0


UPDATE dbo.Chuyende
SET
    Ten = @Ten,
    Mota = @Mota,
    UpdatedAt = @UpdatedAt,
    UpdatedBy = @UpdatedBy
WHERE MaID = @MaID


use TPS_db


SELECT * FROM dbo.Chuyende
SELECT * FROM dbo.Chuyende_Tailieu
SELECT * FROM dbo.Chuyende_Cauhoi
SELECT * FROM dbo.Chuyende_Dapan


UPDATE dbo.Giangvien
SET
    Hoten = @Hoten, Ngaysinh = @Ngaysinh,
    Gioitinh = @Gioitinh, Email = @Email,
    Diachi = @Diachi, Dienthoai = @Dienthoai,
    UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
WHERE MaID = @GiangvienID


INSERT INTO dbo.Khoahoc(MaID, Ten, Mota, Diemdat, ChungchiID, Thu, Thoiluonghoc, Sobuoihoc,
                        Thoiluongthi, Ngaybatdau, Socauhoi,
                        CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy)
VALUES (@MaID, @Ten, @Mota, @Diemdat, @ChungchiID, @Thu, @Thoiluonghoc, @Sobuoihoc,
        @Thoiluongthi, @Ngaybatdau, @Socauhoi,
        @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy, @DeletedAt, @DeletedBy)
