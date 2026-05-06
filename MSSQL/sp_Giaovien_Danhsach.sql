USE db_TPS;
GO

DROP PROCEDURE IF EXISTS sp_Giaovien_Danhsach;
GO

CREATE PROCEDURE sp_Giaovien_Danhsach
	@Giaovien_Ten NVARCHAR(100) = NULL,
	@Chuyende_Ten NVARCHAR(100) = NULL,
	@Khoahoc_Ten NVARCHAR(100) = NULL
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX) = 
	N'SELECT DISTINCT
	t.GiaovienID , t.Hoten AS [Ho ten],
	(
		SELECT cd.TenChuyende
		FROM Chuyende cd
		JOIN Giaovien_Chuyende gv_cd ON cd.ChuyendeID = gv_cd.ChuyendeID
		WHERE gv_cd.Khongsudung = 0
		AND gv_cd.GiaovienID = t.GiaovienID
		FOR JSON PATH
	) AS [Danh sach chuyen de],
	kh.Tenkhoahoc [Khoa hoc] 
	FROM dbo.Giaovien t
	JOIN Giaovien_Khoahoc gv_kh ON t.GiaovienID = gv_kh.GiaovienID
	JOIN Giaovien_Chuyende gv_cd ON t.GiaovienID = gv_cd.GiaovienID
	JOIN Khoahoc kh ON gv_kh.KhoahocID = kh.KhoahocID
	JOIN Chuyende cd ON gv_cd.ChuyendeID = cd.ChuyendeID
	WHERE t.Khongsudung = 0 '

	IF @Giaovien_Ten IS NOT NULL
		SET @sql += N' AND t.Hoten LIKE @p_Giaovien_Ten '

	IF @Chuyende_Ten IS NOT NULL
		SET @sql += N' AND cd.Tenchuyende LIKE @p_Chuyende_Ten '

	IF @Khoahoc_Ten IS NOT NULL
		SET @sql += N' AND kh.Tenkhoahoc LIKE @p_Khoahoc_Ten '

	DECLARE @p_Giaovien_Ten NVARCHAR(100)
    DECLARE @p_Chuyende_Ten NVARCHAR(100)
    DECLARE @p_Khoahoc_Ten NVARCHAR(100)

	SET @p_Giaovien_Ten =
		CASE 
			WHEN @Giaovien_Ten IS NOT NULL THEN  @Giaovien_Ten 
			ELSE NULL 
		END
	SET @p_Chuyende_Ten =
		CASE 
			WHEN @Chuyende_Ten IS NOT NULL THEN @Chuyende_Ten 
			ELSE NULL 
		END
	SET @p_Khoahoc_Ten =
		CASE 
			WHEN @Khoahoc_Ten IS NOT NULL THEN @Khoahoc_Ten
			ELSE NULL 
		END

	PRINT @sql -- In ra câu SQL để debug nếu cần

	EXEC sp_executesql
		@sql,
		N'@p_Giaovien_Ten NVARCHAR(100), @p_Chuyende_Ten NVARCHAR(100), @p_Khoahoc_Ten NVARCHAR(100)',
		@p_Giaovien_Ten, 
		@p_Chuyende_Ten, 
		@p_Khoahoc_Ten

END

