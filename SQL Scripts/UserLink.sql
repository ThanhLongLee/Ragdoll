----------------------------------------------------------------------------------------
CREATE TABLE Links (
    LinkID INT PRIMARY KEY IDENTITY(1,1),
    LinkUrl NVARCHAR(255) NOT NULL,
    ScoreAwarded INT NOT NULL,
    [Description] NVARCHAR(255) -- Mô tả (nếu cần)
);

CREATE TABLE UserLinks (
    UserLinkID INT PRIMARY KEY IDENTITY(1,1),
    UserID BIGINT,
    LinkID INT,
    IsCompleted BIT DEFAULT 0,
    ActionDate DATETIME DEFAULT GETDATE(),
);

go
ALTER PROCEDURE sp_GetUserLinks
    @UserID BIGINT
AS
BEGIN
    SELECT l.LinkID, l.LinkUrl, l.ScoreAwarded, 
           ISNULL(ul.IsCompleted, 0) AS IsCompleted
    FROM Links l
    LEFT JOIN UserLinks ul ON l.LinkID = ul.LinkID AND ul.UserID = @UserID;
END

go

ALTER PROCEDURE sp_UpdateUserScore
    @UserID BIGINT,
    @LinkID INT
AS
BEGIN
    DECLARE @ScoreAwarded INT;

    -- Kiểm tra xem link đã hoàn thành chưa
    IF NOT EXISTS (SELECT 1 FROM UserLinks WHERE UserID = @UserID AND LinkID = @LinkID AND IsCompleted = 1)
    BEGIN
        -- Lấy điểm thưởng từ link
        SELECT @ScoreAwarded = ScoreAwarded 
        FROM Links 
        WHERE LinkID = @LinkID;

        -- Cập nhật điểm số cho user
        UPDATE UserScores
        SET Score = Score + @ScoreAwarded,
            UpdatedAt = GETDATE()
        WHERE UserID = @UserID;

        -- Thêm hoặc cập nhật trạng thái hoàn thành cho UserLink
        IF EXISTS (SELECT 1 FROM UserLinks WHERE UserID = @UserID AND LinkID = @LinkID)
        BEGIN
            UPDATE UserLinks
            SET IsCompleted = 1, ActionDate = GETDATE()
            WHERE UserID = @UserID AND LinkID = @LinkID;
        END
        ELSE
        BEGIN
            INSERT INTO UserLinks (UserID, LinkID, IsCompleted, ActionDate)
            VALUES (@UserID, @LinkID, 1, GETDATE());
        END
    END
    ELSE
    BEGIN
        SET @ScoreAwarded = 0; -- Không có điểm nếu hành động đã hoàn thành
    END

    -- Trả về điểm cộng hoặc 0 nếu đã hoàn thành hành động
    SELECT @ScoreAwarded AS ScoreAwarded;
END


