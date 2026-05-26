
use TPS_db

CREATE PROC dbo.sp_Khoahoc_Thongke
AS
BEGIN

    SELECT
    kh.MaID,
    kh.Ten,
    STRING_AGG(cd.Ten, ', ') AS Danhsachchuyende, 
    COUNT(DISTINCT cdch.MaID) AS Tongsocauhoi,
    COUNT(DISTINCT hv.MaID) AS Tonghocvien,
    COUNT(DISTINCT lh.MaID) AS Tongsobuoi,
    COUNT(DISTINCT CASE
        WHEN lh.Batdauthucte IS NOT NULL
        OR lh.Ketthucthucte IS NOT NULL
        THEN lh.MaID
        END) AS Tongsobuoithucte,
    CASE 
    WHEN COUNT(DISTINCT lh.MaID) = 0 THEN 0
    ELSE 
        (COUNT(DISTINCT CASE 
            WHEN lh.Batdauthucte IS NOT NULL 
              OR lh.Ketthucthucte IS NOT NULL 
            THEN lh.MaID 
        END) * 100.0)
        / COUNT(DISTINCT lh.MaID)
    END AS TyLeHoanThanh
    FROM Khoahoc kh
    
    LEFT JOIN Khoahoc_Chuyende khcd
        ON kh.MaID = khcd.KhoahocID
    LEFT JOIN Chuyende cd 
        ON cd.MaID = khcd.ChuyendeID
    LEFT JOIN Chuyende_Cauhoi cdch
        ON cdch.ChuyendeID = cd.MaID
    LEFT JOIN Chuyende_Dapan cdda
        ON cdda.Chuyende_CauhoiID = cdch.MaID
    LEFT JOIN Baithuhoach bth
        ON bth.KhoahocID = kh.MaID
    LEFT JOIN Baithuhoach_Cauhoi bthch
        ON bthch.BaithuhoachID = bth.MaID
    LEFT JOIN Khoahoc_Hocvien khhv
        ON khhv.KhoahocID = kh.MaID
    LEFT JOIN Hocvien hv
        ON khhv.HocvienID = hv.MaID
    LEFT JOIN Lichhoc lh
        ON kh.MaID = lh.KhoahocID
    GROUP BY kh.MaID, kh.Ten
