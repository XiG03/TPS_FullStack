use TPS_db



UPDATE dbo.Khoahoc
SET
    Khongsudung = 1,
    DeletedAt = @DeletedAt,
    DeletedBy = @DeletedBy
WHERE MaID = @ChungchiID


SELECT MaID, Ten, Mota, Diemdat, Chungchi, Thu, Thoiluonghoc, Sobuoihoc, Ngaybatdau, Thoiluongthi, Socauhoi
FROM dbo.Khoahoc
WHERE Khongsudung = 0 AND MaID = @KhoahocID


UPDATE dbo.Khoahoc
SET
    Ten = @Ten, Mota = @Mota, Diemdat = @Diemdat, ChungchiID = @ChungchiID, 
    Thu = @Thu, Thoiluonghoc = @Thoiluonghoc, Sobuoihoc = @Sobuoihoc, Ngaybatdau = @Ngaybatdau
    Thoiluongthi = @Thoiluongthi, Socauhoi = @Socauhoi,
    UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
WHERE MaID = @KhoahocID



UPDATE dbo.Khoahoc
SET
    ChungchiID = @ChungchiID,
    UpdatedAt = @UpdatedAt,
    UpdatedBy = @UpdatedBy
WHERE MaID = @KhoahocID

INSERT INTO dbo.Chuyende_Giangvien (MaID, ChuyendeID, GiangvienID)
VALUES (@MaID, @ChuyendeID, @GiangvienID)


DELETE dbo.Chuyende_Giangvien
WHERE MaID = @MaID

SELECT MaID, ChuyendeID, GiangvienID
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND ChuyendeID = @ChuyendeID)
OR (@GiangvienID IS NOT NULL AND GiangvienID = @GiangvienID)

SELECT Top 1 MaID, ChuyendeID, GiangvienID
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND ChuyendeID = @ChuyendeID)
OR (@GiangvienID IS NOT NULL AND GiangvienID = @GiangvienID)


UPDATE dbo.Chuyende_Giangvien
SET
    GiangvienID = @GiangvienID,
    ChuyendeID = @ChuyendeID
WHERE MaID = @MaID

SELECT cdgv.MaID, cdgv.GiangvienID, cdgv.ChuyendeID, cd.Ten
FROM dbo.Chuyende_Giangvien cdgv
JOIN dbo.Chuyende cd ON cdgv.ChuyendeID = cd.MaID
WHERE cdgv.GiangvienID = @GiangvienID

use TPS_db

SELECT cdgv.MaID, cdgv.GiangvienID, cdgv.ChuyendeID, cd.Ten
FROM dbo.Chuyende_Giangvien cdgv
JOIN dbo.Chuyende cd ON cdgv.ChuyendeID = cd.MaID
-- WHERE cdgv.GiangvienID = eb12753b-798a-4a67-a2e3-c707f92e89ca