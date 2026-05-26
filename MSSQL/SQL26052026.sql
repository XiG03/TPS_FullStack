use TPS_db


ALTER TABLE dbo.Baithuhoach_Cauhoi
DROP COLUMN DapanID, Nddapan


CREATE TABLE Baithuhoach_Traloi(
    MaID NVARCHAR(50) NULL,
    BaithuhoachID NVARCHAR(50) NULL,
    Baithuhoach_CauhoiID NVARCHAR(50) NULL,
    TraloiID NVARCHAR(50) NULL,
    NdTraloi NVARCHAR(200) NULL,
    Dung BIT NULL
)

ALTER TABLE Baithuhoach_Traloi
ADD Chon BIT NULL



SELECT Top 1 * FROM dbo.Chuyende_Cauhoi
WHERE (@MaID IS NOT NULL AND MaID = @MaID) OR (@ChuyendeID IS NOT NULL AND @ChuyendeID = ChuyendeID)



SELECT * FROM dbo.Lichhoc