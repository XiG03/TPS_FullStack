use TPS_db

ALTER TABLE dbo.Lichhoc_Giangvien_Diemdanh
ADD Vaoluc DATETIME NULL

ALTER TABLE dbo.Lichhoc_Hocvien_Diemdanh
ADD Vaoluc DATETIME NULL


ALTER TABLE dbo.Lichhoc_Giangvien_Diemdanh
ADD Ngaydiemdanh  DATETIME NULL

ALTER TABLE dbo.Lichhoc_Hocvien_Diemdanh
ADD Ngaydiemdanh DATETIME NULL



SELECT * FROM dbo.Lichhoc
ORDER BY GiangvienID


SELECT * FROM dbo.Lichhoc_Giangvien_Diemdanh


SELECT lh.*
FROM dbo.Lichhoc lh
JOIN dbo.Khoahoc kh ON lh.KhoahocID = kh.MaID
JOIN dbo.Khoahoc_Hocvien khhv ON khhv.KhoahocID = kh.MaID
WHERE khhv.HocvienID = 'f1697333-1de0-48a9-b2bd-2e7033e767ec'

SELECT * FROM dbo.Khoahoc_Hocvien
ORDER BY KhoahocID

SELECT *
FROM dbo.Lichhoc lh
WHERE lh.KhoahocID = '5ab371c9-681d-4a54-9714-37b4cd808fd5'



SELECT * 
FROM dbo.Lichhoc_Hocvien_Diemdanh


UPDATE dbo.AspNetUsers
SET
    Kichhoat = 1
WHERE Id = 'a9b92d05-0102-40a8-b5af-c7cf3737d835'

SELECT * FROM dbo.AspNetUsers