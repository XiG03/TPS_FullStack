


DECLARE @tableName NVARCHAR(128);

DECLARE @Output NVARCHAR(50); -- Se chua thong tin cua MaID moi
		@maxNum INT
		

		SELECT TOP 1 @maxNum = SUBSTRING(MaID, CHARINDEX('#',MaID) + 1, LEN(MaID))
		FROM @tableName
		ORDER BY MaID DESC

		@Output = @tableName + '#' + RIGHT ('000' + CAST(@maxNum + 1 AS NVARCHAR(10)), 3)



SELECT MaID
FROM dbo.Chuyende
ORDER BY MaID
DESC