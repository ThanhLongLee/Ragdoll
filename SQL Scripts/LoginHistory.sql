--CREATE TABLE LoginHistory(
--	Id BIGINT IDENTITY(1,1)
--	,UserId  UNIQUEIDENTIFIER NOT NULL
--	,IPAddress VARCHAR(250)
--	,CreatedDate DATETIME

--    CONSTRAINT PK_LoginHistory PRIMARY KEY (Id)
--	,CONSTRAINT fk_LoginHistory_User FOREIGN KEY (UserId) REFERENCES Users(Id)
--)
--GO

--CREATE PROCEDURE pro_LoginHistory_Insert
--@UserId  UNIQUEIDENTIFIER
--,@IPAddress VARCHAR(250)
--,@CreatedDate DATETIME
--AS BEGIN
--	INSERT INTO LoginHistory(UserId, IPAddress, CreatedDate)
--	VALUES(@UserId, @IPAddress, @CreatedDate)
--END
--GO

