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