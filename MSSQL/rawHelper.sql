
use TPS_db


SELECT * FROM dbo.Khoahoc




ALTER TABLE Thuchanh
ADD Diachi NVARCHAR(200) NULL

CREATE TABLE dbo.Thuchanh
(
    MaID NVARCHAR(50) PRIMARY KEY,
    KhoahocID NVARCHAR(50) NULL,
    DiadiemID NVARCHAR(50) NULL,
    GiangvienID NVARCHAR(50) NULL,
    Soluongtoida DECIMAL(18,2) NULL,
    LichhocID NVARCHAR(50) NULL
)

CREATE TABLE dbo.Thuchanh_Dangky
(
    MaID NVARCHAR(50) PRIMARY KEY,
    KhoahocID NVARCHAR(50) NULL,
    ThuchanhID NVARCHAR(50) NULL,
    HocvienID NVARCHAR(50) NULL,
    Dangky BIT NULL
)



CREATE TABLE dbo.Tuluan
(
    MaID NVARCHAR(50) PRIMARY KEY,
    KhoahocID NVARCHAR(50) NULL,
    ChuyendeID NVARCHAR(50) NULL,
    GiangvienID NVARCHAR(50) NULL,
    Ten NVARCHAR(200) NULL,
    Mota NVARCHAR(500) NULL,
)

CREATE TABLE dbo.Tuluan_Hocvien
(
    MaID NVARCHAR(50) PRIMARY KEY,
    KhoahocID NVARCHAR(50) NULL,
    ChuyendeID NVARCHAR(50) NULL,
    TuluanID NVARCHAR(50) NULL,
    HocvienID NVARCHAR(50) NULL,
    GiangvienID NVARCHAR(50) NULL,

)
ALTER TABLE dbo.Thuchanh
DROP COLUMN DiadiemID

ALTER TABLE dbo.Thuchanh
ADD ChuyendeID NVARCHAR(50) NULL




-- query CRUD

INSERT INTO dbo.Thuchanh (MaID, KhoahocID, GiangvienID, LichhocID, ChuyendeID, Diachi, Soluongtoida)
VALUES (@MaID, @KhoahocID, @GiangvienID, @LichhocID, @ChuyendeID, @Diachi, @Soluongtoida)


use TPS_db

SELECT * FROM dbo.Thuchanh
    