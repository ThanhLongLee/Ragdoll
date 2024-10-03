CREATE TABLE Edition(
	Id INT IDENTITY(100,2),
	Title NVARCHAR(500),
	ThumbnailImg NVARCHAR(500),
	BannerImg NVARCHAR(500),
	TitleOnBanner NVARCHAR(500),
	SubTitleOnBanner NVARCHAR(500),

	IntroduceContent NTEXT, -- Content đầu tiên của Edition
	IntroduceThumbnail NVARCHAR(500), -- Dùng để hiển thị phần đầu tiên của Edition Summary

	[Status] TINYINT DEFAULT 0,
	CreatedDate DATETIME DEFAULT GETDATE(),
	ModifiedDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT PK_Edition PRIMARY KEY (Id)
)
GO

-- TOC viết tắt của: Table Of Contents - Mục lục
CREATE TABLE EditionTOC(
	Id INT IDENTITY(1,1),
	EditionId INT NOT NULL,
	Title NVARCHAR(500), -- Tiêu đề mục lục
	[Index] INT DEFAULT 0,

	PostTitle NVARCHAR(500), -- Tiêu đề bài viết
	PostContent NTEXT, -- Nội dung bài viết

	IsShowOnHeader BIT DEFAULT 1, -- Hiển thị trên header
	IsShowOnSummaryList BIT DEFAULT 1, -- Hiển thị trên danh sách Edition Summary

	[Status] TINYINT DEFAULT 0,
	CreatedDate DATETIME DEFAULT GETDATE(),
	ModifiedDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT PK_EditionTOC PRIMARY KEY (Id),
	CONSTRAINT FK_EditionTOC_Edition FOREIGN KEY (EditionId) REFERENCES Edition (Id)
)
GO

-- Thông tin phần Summary của Edition
CREATE TABLE EditionSummary(
	Id INT IDENTITY(1,1),
	EditionTOCId INT NOT NULL,
	ThumbnailImg NVARCHAR(500),
	Title NVARCHAR(500),
	[Description] NVARCHAR(500), -- đoạn tóm tắt

	[Status] TINYINT DEFAULT 0,
	CreatedDate DATETIME DEFAULT GETDATE(),
	ModifiedDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT PK_EditionSummary PRIMARY KEY (Id),
	CONSTRAINT FK_EditionSummary_EditionTOCId FOREIGN KEY (EditionTOCId) REFERENCES EditionTOC (Id)
)
GO


--===================================================
--			 Edition - STORE PROCEDURE
--===================================================

CREATE PROCEDURE pro_Edition_Insert
@CreatedBy UNIQUEIDENTIFIER,
@Title NVARCHAR(500),
@ThumbnailImg NVARCHAR(500),
@BannerImg NVARCHAR(500),
@TitleOnBanner NVARCHAR(500),
@SubTitleOnBanner NVARCHAR(500),
@IntroduceThumbnail NVARCHAR(500), -- Dùng để hiển thị phần đầu tiên của Edition Summary
@IntroduceContent NTEXT, -- Content đầu tiên của Edition
@CreatedDate DATETIME
AS BEGIN
	INSERT INTO Edition(Title, ThumbnailImg, BannerImg, TitleOnBanner, SubTitleOnBanner, IntroduceContent, IntroduceThumbnail, CreatedDate, ModifiedDate)
	VALUES (@Title, @ThumbnailImg, @BannerImg, @TitleOnBanner, @SubTitleOnBanner, @IntroduceContent, @IntroduceThumbnail, @CreatedDate, @CreatedDate);
END
GO

CREATE PROCEDURE pro_Edition_Update
@CreatedBy UNIQUEIDENTIFIER,
@Id INT,
@Title NVARCHAR(500),
@ThumbnailImg NVARCHAR(500),
@BannerImg NVARCHAR(500),
@TitleOnBanner NVARCHAR(500),
@SubTitleOnBanner NVARCHAR(500),
@IntroduceThumbnail NVARCHAR(500), -- Dùng để hiển thị phần đầu tiên của Edition Summary
@IntroduceContent NTEXT, -- Content đầu tiên của Edition
@ModifiedDate DATETIME
AS BEGIN
	UPDATE Edition 
	SET Title = @Title,
		ThumbnailImg = @ThumbnailImg,
		BannerImg = @BannerImg,
		TitleOnBanner = @TitleOnBanner,
		SubTitleOnBanner = @SubTitleOnBanner,
		IntroduceContent = @IntroduceContent,
		IntroduceThumbnail = @IntroduceThumbnail,
		ModifiedDate = @ModifiedDate
	WHERE Id = @Id
END
GO

CREATE PROCEDURE pro_Edition_FindById
@Id INT
AS BEGIN
	SELECT * FROM Edition WHERE Id = @Id
END
GO

CREATE PROCEDURE pro_Edition_GetAll
@Status TINYINT
AS BEGIN
	IF @Status != 250 BEGIN
		SELECT * FROM Edition WHERE [Status] = @Status
	END ELSE BEGIN
		SELECT * FROM Edition
	END
END
GO


CREATE PROCEDURE pro_Edition_FindBy
@Keyword nvarchar(256)
,@Status tinyint
,@BeginRow int
,@NumRows int
AS BEGIN
	DECLARE @SQL nvarchar(max), @WHERE nvarchar(2000), @OrderBy nvarchar(500), @OrderExpression varchar(10);
		
	SET @OrderExpression = 'ASC'

	

	SET @SQL = 'WITH PRO_PAGIN AS (SELECT ROW_NUMBER() OVER (ORDER BY e.CreatedDate DESC) AS RowNum, COUNT(*) OVER () AS TotalRows,
						e.*
						FROM Edition e '

	SET @WHERE = ''

	IF @Status != 254
		SET @WHERE = @WHERE + 'AND up.[Status] = @Status '

	IF @Keyword IS NOT NULL AND LEN(@Keyword) > 0
		SET @WHERE = @WHERE + 'AND CONTAINS(e.Title, @Keyword)'
	
	IF @WHERE IS NOT NULL AND LEN(@WHERE) > 0
		SET @SQL = @SQL + ' WHERE ' + RIGHT(@WHERE, LEN(@WHERE) - 3)

	SET @SQL = @SQL + ' ) SELECT pa.* FROM PRO_PAGIN pa '

	IF @NumRows != 0
		SET @SQL = @SQL + 'WHERE RowNum BETWEEN @BeginRow AND @NumRows ORDER BY RowNum ASC '

	EXEC sp_executesql @SQL 
	, N'@Keyword nvarchar(250)	
		,@Status tinyint
		,@BeginRow int
		,@NumRows int',
		@Keyword
		,@Status		
		,@BeginRow
		,@NumRows
END
GO


--===================================================
--			 EditionTOC - STORE PROCEDURE
--===================================================

CREATE PROCEDURE pro_EditionTOC_Insert
@CreatedBy UNIQUEIDENTIFIER,
@EditionId INT,
@Title NVARCHAR(500), -- Tiêu đề mục lục
@PostTitle NVARCHAR(500), -- Tiêu đề bài viết
@PostContent NTEXT, -- Nội dung bài viết
@IsShowOnHeader BIT, -- Hiển thị trên header
@IsShowOnSummaryList BIT, -- Hiển thị trên danh sách Edition Summary
@CreatedDate DATETIME
AS BEGIN
	DECLARE @SortIndex INT;
	SET @SortIndex = ISNULL((SELECT TOP 1 [Index] FROM EditionTOC WHERE EditionId = @EditionId ORDER BY [Index] DESC), 0) + 1

	INSERT INTO EditionTOC(EditionId, Title, [Index], PostTitle, PostContent, IsShowOnHeader, IsShowOnSummaryList, CreatedDate, ModifiedDate)
	VALUES (@EditionId, @Title, @SortIndex, @PostTitle, @PostContent, @IsShowOnHeader, @IsShowOnSummaryList, @CreatedDate, @CreatedDate);
END
GO

CREATE PROCEDURE pro_EditionTOC_Update
@CreatedBy UNIQUEIDENTIFIER,
@Id INT,
@Title NVARCHAR(500), -- Tiêu đề mục lục
@PostTitle NVARCHAR(500), -- Tiêu đề bài viết
@PostContent NTEXT, -- Nội dung bài viết
@IsShowOnHeader BIT, -- Hiển thị trên header
@IsShowOnSummaryList BIT, -- Hiển thị trên danh sách Edition Summary
@ModifiedDate DATETIME
AS BEGIN
	UPDATE EditionTOC 
	SET Title = @Title,
	 PostTitle = @PostTitle,
	 PostContent = @PostContent,
	 IsShowOnHeader = @IsShowOnHeader,
	 IsShowOnSummaryList = @IsShowOnSummaryList,
	 ModifiedDate = @ModifiedDate
	 WHERE Id = @Id
END
GO

CREATE PROCEDURE pro_EditionTOC_Delete
@CreatedBy UNIQUEIDENTIFIER,
@Id INT,
@ModifiedDate DATETIME
AS BEGIN
SET XACT_ABORT ON
BEGIN TRAN
	DECLARE @ObjectId VARCHAR(128), @Keys nvarchar(1000)

	DELETE FROM EditionSummary WHERE EditionTOCId = @Id
	DELETE FROM EditionTOC WHERE Id = @Id
			
	SET @Keys = '@Id: ' + CAST(@Id AS nvarchar(256))

	SET @ObjectId = CAST(@Id AS VARCHAR(128));
	EXEC pro_DataLog_Insert
				@TableName		= 'EditionTOC'
				, @Operation	= 'DELETE'
				, @ObjectId		= @ObjectId
				, @Content		= @Keys
				, @UserId		= @CreatedBy
				,@CreatedDate	= @ModifiedDate
COMMIT
END
GO

CREATE PROCEDURE pro_EditionTOC_UpdateIndex
@CreatedBy UNIQUEIDENTIFIER,
@SortTable list_sort_index READONLY,
@ModifiedDate DATETIME
AS BEGIN
	UPDATE EditionTOC
    SET EditionTOC.[Index] = Temp.[Index]
    FROM EditionTOC
    INNER JOIN @SortTable Temp ON EditionTOC.Id = Temp.Id
END
GO

CREATE PROCEDURE pro_EditionTOC_FindByEdition
@EditionId INT
,@Status TINYINT
AS BEGIN
	IF @Status != 250 BEGIN
		SELECT * FROM EditionTOC WHERE EditionId = @EditionId AND [Status] = @Status ORDER BY [Index] ASC
	END ELSE BEGIN
		SELECT * FROM EditionTOC WHERE EditionId = @EditionId ORDER BY [Index] ASC
	END
END
GO

CREATE PROCEDURE pro_EditionTOC_FindById
@Id INT
AS BEGIN
	SELECT * FROM EditionTOC WHERE Id = @Id
END
GO


--===================================================
--			 EditionSummary - STORE PROCEDURE
--===================================================

CREATE PROCEDURE pro_EditionSummary_Insert
@EditionTOCId INT,
@ThumbnailImg NVARCHAR(500),
@Title NVARCHAR(500),
@Description NVARCHAR(500), -- đoạn tóm tắt
@CreatedDate DATETIME
AS BEGIN
	INSERT INTO EditionSummary(EditionTOCId, Title, ThumbnailImg, [Description], CreatedDate, ModifiedDate)
	VALUES (@EditionTOCId, @Title, @ThumbnailImg, @Description, @CreatedDate, @CreatedDate);
END
GO

CREATE PROCEDURE pro_EditionSummary_Update
@EditionTOCId INT,
@ThumbnailImg NVARCHAR(500),
@Title NVARCHAR(500),
@Description NVARCHAR(500), -- đoạn tóm tắt
@ModifiedDate DATETIME
AS BEGIN
	UPDATE EditionSummary 
	SET Title = @Title,
		ThumbnailImg = @ThumbnailImg,
		[Description] = @Description,
		ModifiedDate = @ModifiedDate
	 WHERE EditionTOCId = @EditionTOCId
END
GO

CREATE PROCEDURE pro_EditionSummary_FindByEditionTOC
@EditionTOCId INT
AS BEGIN
	SELECT * FROM EditionSummary WHERE EditionTOCId = @EditionTOCId
END
GO

CREATE PROCEDURE pro_EditionSummary_FindByEdition
@EditionId INT
AS BEGIN
	SELECT es.* FROM EditionSummary es
	INNER JOIN EditionTOC etoc ON es.EditionTOCId = etoc.Id
	INNER JOIN Edition e ON etoc.EditionId = e.Id
	WHERE e.Id = @EditionId ORDER BY etoc.[Index] ASC
END
GO