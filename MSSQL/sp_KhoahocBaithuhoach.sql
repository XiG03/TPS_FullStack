use TPS_db

CREATE PROCEDURE sp_KhoahocBaithuhoach
    @MaKhoahoc NVARCHAR(50)
AS
BEGIN
    DECLARE @Socauthucte DECIMAL(18,2)

    SELECT @Socauthucte = COUNT(*)
    FROM dbo.Khoahoc_Chuyende
    JOIN dbo.Chuyende ON dbo.Khoahoc_Chuyende.MaCD = dbo.Chuyende.MaCD
    WHERE dbo.Khoahoc_Chuyende.MaKH = @MaKhoahoc
END