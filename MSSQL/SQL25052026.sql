use TPS_db


SELECT  * FROM dbo.AspNetUsers



SELECT MaID, KhoahocID, kh.Ten, ChuyendeID, cd.Ten, GiangvienID, gv.Hoten, 
            Ngaydukien, Batdaudukien, Ketthucdukien,
            Ngaythucte, Batdauthucte, Ketthucthucte
FROM dbo.Lichhoc lh
JOIN dbo.Khoahoc kh on kh.MaID = lh.KhoahocID
JOIN dbo.Giangvien gv ON gv.MaID = lh.GiangvienID
JOIN dbo.Chuyende cd ON cd.MaID = lh.ChuyendeID 



SELECT 
    lh.MaID,

    lh.KhoahocID,
    kh.Ten AS TenKhoaHoc,

    lh.ChuyendeID,
    cd.Ten AS TenChuyenDe,

    lh.GiangvienID,
    gv.Hoten AS TenGiangVien,

    lh.Ngaydukien,
    lh.Batdaudukien,
    lh.Ketthucdukien,

    lh.Ngaythucte,
    lh.Batdauthucte,
    lh.Ketthucthucte

FROM dbo.Lichhoc lh
JOIN dbo.Khoahoc kh 
    ON kh.MaID = lh.KhoahocID

JOIN dbo.Giangvien gv 
    ON gv.MaID = lh.GiangvienID

JOIN dbo.Chuyende cd 
    ON cd.MaID = lh.ChuyendeID;


SELECT * FROM dbo.Hocvien