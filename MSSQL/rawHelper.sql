
-- MaID duoc cau thanh tu: Ten bang # STT
CREATE PROCEDURE dbo.sp_genIdHelper
	@tableName NVARCHAR(128),
	@Output NVARCHAR(50) OUTPUT
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @maxNum INT

		SELECT TOP 1 @maxNum = CAST(SUBSTRING(MaID, CHARINDEX('#', MaID) + 1, LEN(MaID)) AS INT)
		FROM QUOTENAME(@tableName)
		ORDER BY MaID DESC

		SET @maxNum = ISNULL(@maxNum, 0);

		SET @Output = @tableName + '#' + RIGHT('000' + CAST(@maxNum + 1 AS NVARCHAR(10)), 3)

	END
