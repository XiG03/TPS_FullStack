INSERT INTO Chuyende(MaID, Ten, Mota, Khongsudung, UpdatedAt, UpdatedBy, CreatedAt, CreatedBy)
VALUES (NEWID(),@Topicname, @Description,0,SYSDATETIME(), NEWID(),SYSDATETIME(),NEWID())

SELECT * FROM dbo.Chuyende


INSERT INTO Chuyende_Tailieu(MaID, ChuyendeID, Tieude, Ngaytao, Loaitailieu, Kichthuoc, Khongsudung)
VALUES (NEWID(),NEWID(),'Tieude',SYSDATETIME(), 'File,Video', 3.2,0)

SELECT * FROM dbo.Chuyende_Tailieu


INSERT INTO Chuyende_Cauhoi(MaID, ChuyendeID, Ten, Diem)
VALUES (NEWID(),NEWID(),'TEN CAU HOI', 3.2)

SELECT * FROM dbo.Chuyende_Cauhoi

INSERT INTO Chuyende_Dapan(MaID, Chuyende_CauhoiID, Ten, Dung)
VALUES (NEWID(), NEWID(), 'TEN', 1)

SELECT * FROM dbo.Chuyende_Dapan
SELECT * FROM dbo.Chuyende_Cauhoi
SELECT * FROM dbo.Chuyende_Tailieu

use TPS_db


SELECT cd.MaID AS ChuyendeID, cd.Ten AS Tenchuyende, cd.Mota,
cd_tl.MaID AS TailieuID ,cd_tl.Tieude, cd_tl.Loaitailieu,
cd_ch.MaID AS CauhoiID, cd_ch.Ten AS Tencauhoi,
cd_da.MaID AS DapanID,cd_da.Ten AS Tendapan, cd_da.Dung,
gv.Hoten
FROM dbo.Chuyende cd
LEFT JOIN dbo.Chuyende_Giangvien cd_gv ON cd.MaID = cd_gv.ChuyendeID
LEFT JOIN dbo.Giangvien gv ON cd_gv.GiangvienID = gv.MaID
JOIN dbo.Chuyende_Tailieu cd_tl ON cd.MaID = cd_tl.ChuyendeID
JOIN dbo.Chuyende_Cauhoi cd_ch ON cd.MaID = cd_ch.ChuyendeID
JOIN dbo.Chuyende_Dapan cd_da ON cd_ch.MaID = cd_da.Chuyende_CauhoiID
--WHERE cd.MaID = '';



UPDATE dbo.Chuyende
SET 
	Khongsudung = 1,
	DeletedAt = SYSDATETIME(),
	DeletedBy = NEWID()
WHERE MaID = 'Chuyende#001'



SELECT * FROM dbo.Chuyende

