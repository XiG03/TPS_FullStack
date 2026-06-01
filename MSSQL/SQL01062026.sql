use TPS_db


select gv.*, lh.MaID as LichhocID, lh.KhoahocID, lhd.Vaoluc, lhd.Ngaydiemdanh
from dbo.Giangvien gv
join dbo.Lichhoc lh on gv.MaID = lh.GiangvienID
join dbo.Lichhoc_Giangvien_Diemdanh lhd on lhd.LichhocID = lh.MaID
where gv.MaID = 'a88e84b2-0dd5-473f-a67d-be009dde8278'

select *
from dbo.RefreshTokens
where refreshToken = @refreshToken


select * from dbo.AspNetUsers



ALTER TABLE dbo.Chuyende_Tailieu
ADD TentepGoc NVARCHAR(200) NULL,
    Duongdan NVARCHAR(500) NULL


SELECT * FROM dbo.AspNetUsers

update dbo.AspNetUsers
set Kichhoat = 1
where Id = '99766ca2-9bdb-4ccd-86be-a1fe7deb5ad1'


select * from dbo.Lichhoc_Hocvien_Diemdanh


SELECT lh.*, kh.Ten AS TenKhoahoc 
FROM dbo.Lichhoc lh
JOIN dbo.Khoahoc kh ON lh.KhoahocID = kh.MaID
JOIN dbo.Lichhoc_Hocvien_Diemdanh lhd ON lhd.LichhocID = lh.MaID
JOIN dbo.Khoahoc_Hocvien khhv ON khhv.KhoahocID = kh.MaID
WHERE khhv.HocvienID = @HocvienID


CREATE PROCEDURE [dbo].[GetLichhocByHocvienID]
    @HocvienID NVARCHAR(50)
AS
BEGIN
    SELECT
        lh.*,
        kh.Ten AS TenKhoahoc,
        CASE
            WHEN EXISTS (
                SELECT 1
                FROM dbo.Lichhoc_Hocvien_Diemdanh lhd
                WHERE lhd.LichhocID = lh.MaID
                  AND lhd.HocvienID = @HocvienID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END AS Trangthai
    FROM dbo.Lichhoc lh
    JOIN dbo.Khoahoc kh
        ON lh.KhoahocID = kh.MaID
    JOIN dbo.Khoahoc_Hocvien khhv
        ON khhv.KhoahocID = kh.MaID
    WHERE khhv.HocvienID = @HocvienID
END


EXEC dbo.GetLichhocByHocvienID @HocvienID = 'f1697333-1de0-48a9-b2bd-2e7033e767ec'