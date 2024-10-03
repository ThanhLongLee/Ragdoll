CREATE TABLE UserInvites
(
    InviteID INT PRIMARY KEY IDENTITY(1,1),
    SenderID BIGINT,
    ReceiverID BIGINT,
    IsSuccessful BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);

ALTER PROCEDURE sp_HandleInvite
    @SenderID BIGINT,
    @ReceiverID BIGINT,
    @ReceiverFirstname NVARCHAR(100),
    @ReceiverLastname NVARCHAR(100),
    @ReceiverUsername NVARCHAR(100)
AS
BEGIN
    -- Kiểm tra nếu người nhận (ReceiverID) đã có trong cơ sở dữ liệu
    IF NOT EXISTS (SELECT 1 FROM UserTelegram WHERE Id = @ReceiverID)
    BEGIN
        -- Thêm người dùng mới vào UserTelegram
        INSERT INTO UserTelegram (Id, Firstname, Lastname, Username, Click)
        VALUES (@ReceiverID, @ReceiverFirstname, @ReceiverLastname, @ReceiverUsername, 1500);

		DECLARE @MinRankID INT;
        SELECT TOP 1 @MinRankID = Id
        FROM Ranks
        ORDER BY MinPoint ASC;

        -- Lưu điểm khởi đầu và cấp bậc thấp nhất vào bảng UserScores
        INSERT INTO UserScores (UserID, Score, RankID)
        VALUES (@ReceiverID, 0, @MinRankID);
    END

    -- Kiểm tra xem lời mời đã được gửi từ SenderID đến ReceiverID chưa
    IF NOT EXISTS (SELECT 1 FROM UserInvites WHERE SenderID = @SenderID AND ReceiverID = @ReceiverID)
    BEGIN
        -- Cập nhật điểm cho người gửi lời mời
        UPDATE UserScores
        SET Score = Score + 5000
        WHERE UserID = @SenderID;

        -- Đánh dấu lời mời là thành công
        INSERT INTO UserInvites (SenderID, ReceiverID, IsSuccessful)
        VALUES (@SenderID, @ReceiverID, 1);
    END
    ELSE
    BEGIN
        -- Lời mời đã tồn tại, không thực hiện cập nhật điểm và đánh dấu lại
        UPDATE UserInvites
        SET IsSuccessful = 1
        WHERE SenderID = @SenderID AND ReceiverID = @ReceiverID;
    END
END;


EXEC sp_HandleInvite 7265672624, 555, 'test', 'test', 'test'
