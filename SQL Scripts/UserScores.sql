CREATE TABLE UserScores (
    ScoreID INT PRIMARY KEY IDENTITY(1,1),
    UserID BIGINT,
    Score INT NOT NULL,
    RankID INT,
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE PROCEDURE pro_UpdateUserScore
    @UserId BIGINT
AS
BEGIN
    DECLARE @CurrentScore INT;
    DECLARE @NewScore INT;
    DECLARE @RankId INT;
    DECLARE @PointAdd INT;

    -- Lấy điểm hiện tại và RankID của người dùng
    SELECT @CurrentScore = Score, @RankId = RankID
    FROM UserScores
    WHERE UserID = @UserId;

    -- Lấy PointAdd từ bảng Rank dựa trên RankID hiện tại
    SELECT @PointAdd = PointAdd
    FROM Ranks
    WHERE Id = @RankId;

    -- Cộng điểm
    SET @NewScore = @CurrentScore + @PointAdd;

    -- Cập nhật điểm mới vào bảng UserScores
    UPDATE UserScores
    SET Score = @NewScore, UpdatedAt = GETDATE()
    WHERE UserID = @UserId;

    -- Kiểm tra nếu điểm mới vượt qua MaxPoint của Rank hiện tại, cập nhật RankID
    UPDATE us
    SET us.RankID = r.Id
    FROM UserScores us
    JOIN Ranks r ON @NewScore BETWEEN r.MinPoint AND r.MaxPoint
    WHERE us.UserID = @UserId;

    -- Trả về thông tin đã cập nhật
    SELECT 
        us.Score,
        r.RankName,
        r.PointAdd
    FROM 
        UserScores us
    JOIN 
        Ranks r ON us.RankID = r.Id
    WHERE 
        us.UserID = @UserId;
END
GO

CREATE PROCEDURE pro_GetTop10Users
AS
BEGIN
    WITH RankedUsers AS
    (
        SELECT 
            ROW_NUMBER() OVER (ORDER BY us.Score DESC) AS RowNum,
            ut.Firstname,
            ut.Lastname,
            us.Score
        FROM 
            UserTelegram ut
        JOIN 
            UserScores us ON ut.Id = us.UserID
    )
    SELECT 
        RowNum,
        Firstname,
        Lastname,
        Score
    FROM 
        RankedUsers
    WHERE 
        RowNum <= 10;
END
GO

EXEC pro_GetTop10Users

