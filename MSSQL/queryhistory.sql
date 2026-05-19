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



                


