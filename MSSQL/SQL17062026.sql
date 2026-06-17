
SELECT *
FROM Baithuhoach bth
JOIN Baithuhoach_Cauhoi bthch ON bth.MaID = bthch.BaithuhoachID
JOIN Baithuhoach_Traloi bthtl ON bthch.MaID = bthtl.Baithuhoach_CauhoiID
WHERE bth.MaID = '63bdce62-dd10-456c-b2ac-a0c8cb3b6d43' AND bth.HocvienID = '5d186a89-1ecb-483b-b0b6-2851c54cf7d5'


SELECT * FROM Baithuhoach


SELECT * FROM Baithuhoach
WHERE HocvienID = '5d186a89-1ecb-483b-b0b6-2851c54cf7d5'


UPDATE Baithuhoach_Traloi
SET
    Chon = @Chon
WHERE MaID = @MaID