create TABLE UserTelegram
(
	Id BIGINT PRIMARY KEY,
	Firstname NVARCHAR(100),
	Lastname NVARCHAR(100),
	Username NVARCHAR(100),
	Click INT,
)

go
-- Thủ tục Insert hoặc Cập Nhật thông tin người dùng
CREATE PROCEDURE pro_InsertOrUpdateUser
    @Id BIGINT,
    @Firstname NVARCHAR(100),
    @Lastname NVARCHAR(100),
    @Username NVARCHAR(100),
	@Click INT INT
AS
BEGIN
    -- Kiểm tra nếu người dùng đã tồn tại
    IF EXISTS (SELECT 1 FROM UserTelegram WHERE Id = @Id)
    BEGIN
        -- Nếu người dùng đã tồn tại, cập nhật thông tin người dùng
        UPDATE UserTelegram
        SET Firstname = @Firstname,
            Lastname = @Lastname,
            Username = @Username
        WHERE Id = @Id;
    END
    ELSE
    BEGIN
        -- Nếu người dùng chưa tồn tại, thêm mới người dùng vào bảng UserTelegram
        INSERT INTO UserTelegram (Id, Firstname, Lastname, Username, Click)
        VALUES (@Id, @Firstname, @Lastname, @Username, @Click);

        -- Thêm điểm và cấp bậc thấp nhất
        DECLARE @MinRankID INT;
        SELECT TOP 1 @MinRankID = Id
        FROM Ranks
        ORDER BY MinPoint ASC;

        -- Lưu điểm khởi đầu và cấp bậc thấp nhất vào bảng UserScores
        INSERT INTO UserScores (UserID, Score, RankID)
        VALUES (@Id, 0, @MinRankID);
    END
END
GO


-- Thủ tục lấy thông tin người dùng
alter PROCEDURE pro_GetUserInfo
    @Id BIGINT
AS
BEGIN
    -- Lấy thông tin người dùng
    ;WITH UserRank AS (
        SELECT 
            ut.Firstname,
            ut.Lastname,
            us.Score,
            r.RankName,
			ut.Click,
			r.MaxPoint,
			r.MinPoint,
            (SELECT COUNT(*) FROM Ranks) AS RankCount,
            ROW_NUMBER() OVER (ORDER BY r.MinPoint) AS RankPosition
        FROM 
            UserTelegram ut
        JOIN 
            UserScores us ON ut.Id = us.UserID
        JOIN 
            Ranks r ON us.Score BETWEEN r.MinPoint AND r.MaxPoint
        WHERE 
            ut.Id = @Id
    )
    SELECT 
        ur.Firstname,
        ur.Lastname,
        ur.Score,
        ur.RankName AS RankName,
        ur.RankCount,
		ur.Click,
		ur.MaxPoint,
		ur.MinPoint,
        (SELECT COUNT(*) FROM Ranks r WHERE us.Score >= r.MinPoint) AS CurrentRankPosition
    FROM 
        UserRank ur
    CROSS APPLY
        (SELECT * FROM UserScores us WHERE us.UserID = @Id) us
    WHERE 
        ur.Score BETWEEN (SELECT MinPoint FROM Ranks WHERE RankName = ur.RankName) AND 
        (SELECT MaxPoint FROM Ranks WHERE RankName = ur.RankName);
END

go
CREATE PROCEDURE pro_GetUserRank
    @UserId BIGINT
AS
BEGIN
    SELECT 
        RowNum,
        Firstname,
        Lastname,
        Score
    FROM 
    (
        SELECT 
            ROW_NUMBER() OVER (ORDER BY us.Score DESC) AS RowNum,
            ut.Firstname,
            ut.Lastname,
            us.Score,
            ut.Id
        FROM 
            UserTelegram ut
        JOIN 
            UserScores us ON ut.Id = us.UserID
    ) AS RankedUsers
    WHERE 
        Id = @UserId;
END
GO

EXEC pro_GetUserRank 7265672624

EXEC pro_GetUserInfo 7265672624

SELECT * FROM dbo.UserScores
SELECT * FROM dbo.UserTelegram

SELECT * FROM dbo.Ranks

SELECT * FROM dbo.UserClicks




