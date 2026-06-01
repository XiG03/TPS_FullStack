use TPS_db


select gv.*, lh.MaID as LichhocID, lh.KhoahocID, lhd.Vaoluc, lhd.Ngaydiemdanh
from dbo.Giangvien gv
join dbo.Lichhoc lh on gv.MaID = lh.GiangvienID
join dbo.Lichhoc_Giangvien_Diemdanh lhd on lhd.LichhocID = lh.MaID
where gv.MaID = 'a88e84b2-0dd5-473f-a67d-be009dde8278'

select *
from dbo.RefreshTokens
where refreshToken = @refreshToken