use TPS_db


ALTER TABLE dbo.Baithuhoach
DROP COLUMN Diem


ALTER TABLE dbo.Baithuhoach_Cauhoi
ADD
    Tencauhoi NVARCHAR(200) NULL,
    Nddapan NVARCHAR(200) NULL,
    NdTraloi NVARCHAR(200) NULL;

ALTER TABLE dbo.Baithuhoach
ADD
    Batdauthi Datetime null,
    Ketthucthi Datetime null;


SELECT cdch.ChuyendeID, cdch.MaID AS CauhoiID, cdch.Ten AS Tencauhoi, cdcd.MaID AS DapanID, cdcd.Ten AS Nddapan
FROM dbo.Chuyende_Cauhoi cdch
    JOIN dbo.Chuyende_Dapan cdcd ON cdch.MaID = cdcd.Chuyende_CauhoiID
WHERE cdch.ChuyendeID IN (SELECT ChuyendeID
    FROM dbo.Khoahoc_Chuyende
    WHERE KhoahocID = @KhoahocID) AND cdcd.Dung = 1
ORDER BY cdch.ChuyendeID, cdch.


ALTER TABLE dbo.Baithuhoach_Cauhoi
ADD KhoahocID NVARCHAR(50) NULL



INSERT INTO dbo.Baithuhoach
    (MaID, KhoahocID, HocvienID, Thoigianlambai, CreatedAt, CreatedBy)
VALUES
    (@BaithuhoachID, @KhoahocID, @HocvienID, @Thoigianlambai, @CreatedAt, @CreatedBy)

INSERT INTO dbo.Baithuhoach_Cauhoi
    (MaID, BaithuhoachID, KhoahocID, ChuyendeID, CauhoiID, Tencauhoi, DapanID, Nddapan)
VALUES
    (@Baithuhoach_CauhoiID, @BaithuhoachID, @KhoahocID, @ChuyendeID, @CauhoiID, @Tencauhoi, @DapanID, @Nddapan)



SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, hv.Hoten, bth.Thoigianlambai, (SELECT COUNT(*)
    FROM dbo.Baithuhoach_Cauhoi
    WHERE BaithuhoachID = bth.MaID AND Dung = 1) / (SELECT COUNT(*)
    FROM dbo.Baithuhoach_Cauhoi
    WHERE BaithuhoachID = bth.MaID) AS Diem
FROM dbo.Baithuhoach bth
    JOIN dbo.Hocvien hv ON bth.HocvienID = hv.MaID

DROP FUNCTION dbo.afn_Baithuhoach_Tinhdiem;


CREATE FUNCTION dbo.fn_Baithuhoach_Tinhdiem
(
    @BaithuhoachID NVARCHAR(50)
)
RETURNS DECIMAL(18,2)
AS
BEGIN

    DECLARE @Socaudung DECIMAL(18,2)
    DECLARE @Socauhoi DECIMAL(18,2)
    DECLARE @Diem DECIMAL(18,2)

    SELECT @Socaudung = SUM (CASE WHEN Dung = 1 THEN 1 ELSE 0 END), @Socauhoi = COUNT(*)
    FROM dbo.Baithuhoach_Cauhoi
    WHERE BaithuhoachID = @BaithuhoachID

    IF @Socauhoi = 0
        SET @Diem = 0
    ELSE
        SET @Diem = (@Socaudung/ @Socauhoi) * 10

    RETURN @Diem
END

SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, hv.Hoten AS Tenhocvien, bth.Thoigianlambai, dbo.fn_Baithuhoach_Tinhdiem(bth.MaID) AS Diem
FROM dbo.Baithuhoach bth
    JOIN dbo.Hocvien hv ON bth.HocvienID = hv.MaID 




SELECT bth.MaID, bth.KhoahocID, bth.HocvienID, bth.HocvienID, 
(SELECT Top 1 Hoten FROM dbo.Hocvien WHERE bth.HocvienID = MaID) AS Tenhocvien, 
bth.Thoigianlambai, bth.Batdauthi, bth.Ketthucthi, dbo.fn_Baithuhoach_Tinhdiem(bth.MaID) AS Diem
FROM dbo.Baithuhoach bth
WHERE bth.MaID = @MaID


SELECT btch.MaID, btch.BaithuhoachID, btch.KhoahocID, btch.ChuyendeID, btch.CauhoiID, btch.Tencauhoi, btch.TraloiID, btch.NdTraloi, btch.DapanID, btch.Nddapan, btch.Dung
FROM dbo.Baithuhoach_Cauhoi btch




SELECT bth.MaID, bth.KhoahocID, bth.HocvinienID,
FROM dbo.Baithuhoach bth
WHERE bth.MaID = @MaID

SELECT
FROM dbo.Baithuhoach_Cauhoi bthch
WHERE 

SELECT cdcd.MaID, cdcd.CauhoiID, cdcd.Ten AS Ten
FROM dbo.Chuyende_Dapan cdcd
WHERE dc.cd.CauhoiID = @CauhoiID


use TPS_db

                
ALTER TABLE dbo.Baithuhoach
DROP COLUMN Thoigianlambai

UPDATE dbo.Baithuhoach bth
    SET
        bth.Batdauthi = @Batdauthi,
        bth.Ketthucthi = @Ketthucthi,
WHERE bth.MaID = @BaithuhoachID AND bth.HocvienID = @HocvienID

UPDATE dbo.Baithuhoach_Cauhoi
SET
    TraloiID = @TraloiID,
    NdTraloi = @NdTraloi,
    Dung = CASE 
               WHEN @TraloiID = DapanID THEN 1 
               ELSE 0 
           END
WHERE 
    -- Nhớ thêm điều kiện WHERE để chỉ update đúng dòng dữ liệu bài làm của học sinh
    BaithuhoachID = @BaithuhoachID 
    AND CauhoiID = @CauhoiID;


SELECT dbo.fn_Baithuhoach_Tinhdiem(@BaithuhoachID) AS Diem
FROM dbo.Baithuhoach bth
WHERE bth.MaID = @BaithuhoachID







CREATE PROC dbo.sp_Khoahoc_Hocvien_Tinhdiem
(
    @KhoahocID NVARCHAR(50),
    @HocvienID NVARCHAR(50)
)
AS
BEGIN
    DECLARE @Tongdiem DECIMAL(18,2)
    DECLARE @Tongsobai DECIMAL(18,2)

    -- Dùng ISNULL để bọc SUM lại, đảm bảo @Tongdiem luôn là số (ít nhất là 0)
    SELECT 
        @Tongdiem = ISNULL(SUM(dbo.fn_Baithuhoach_Tinhdiem(MaID)), 0), 
        @Tongsobai = COUNT(*)
    FROM dbo.Baithuhoach
    WHERE KhoahocID = @KhoahocID AND HocvienID = @HocvienID;

    UPDATE dbo.Khoahoc_Hocvien
    SET Diem = CASE 
                   WHEN @Tongsobai = 0 THEN 0
                   ELSE @Tongdiem / @Tongsobai
               END
    WHERE KhoahocID = @KhoahocID AND HocvienID = @HocvienID;
END



CREATE PROC dbo.sp_Thongtin_Khoahoc_Ngay(
    @Ngay DATETIME
)
AS
BEGIN
     













SELECT lh.Ngaydukien, lh.Ngaythucte, ISNULL(COUNT(DISTINCT hvdd.HocvienID), 0) AS Sohocvien, ISNULL(COUNT(DISTINCT gvdd.GiangvienID), 0) AS Sogiangvien
FROM dbo.Lichhoc lh
JOIN dbo.Lichhoc_Hocvien_Diemdanh hvdd ON lh.MaID = hvdd.LichhocID
JOIN dbo.Lichhoc_Giangvien_Diemdanh gvdd ON lh.MaID = gvdd.LichhocID
WHERE lh.Ngay < @Ngay

UNION ALL

SELECT kh.Ten AS TenKhoahoc, cd.Ten AS TenChuyende, (SELECT COUNT(*) FROM dbo.Khoahoc_Hocvien WHERE KhoahocID = kh.MaID) AS Sohocvien
FROM dbo.Khoahoc kh
JOIN dbo.Khoahoc_Chuyende khcd ON kh.MaID = khcd.KhoahocID
JOIN dbo.Chuyende cd ON khcd.ChuyendeID = cd.MaID
WHERE kh.Ngaybatdau < @Ngay