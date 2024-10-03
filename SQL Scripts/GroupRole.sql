--CREATE TABLE GroupRole(
--	Id UNIQUEIDENTIFIER NOT NULL
--	,[Name] NVARCHAR(250)
--	,[Description] NVARCHAR(1000)
--	,[Status] TINYINT DEFAULT 0

--	,ModifiedDate DATETIME
--	,CreatedDate DATETIME

--    CONSTRAINT PK_GroupRole PRIMARY KEY (Id)
--)
--GO


--CREATE TABLE UsersInGroupRole(
--	GroupRoleId UNIQUEIDENTIFIER NOT NULL
--	,UserId UNIQUEIDENTIFIER NOT NULL
--	,[Status] TINYINT DEFAULT 0

--	,ModifiedDate DATETIME
--	,CreatedDate DATETIME

--    ,CONSTRAINT PK_UsersInGroupRole PRIMARY KEY (GroupRoleId, UserId)
--    ,CONSTRAINT FK_UserGroup_User FOREIGN KEY (UserId) REFERENCES Users (Id)
--    ,CONSTRAINT FK_UserGroup_Group FOREIGN KEY (GroupRoleId) REFERENCES GroupRole (Id)
--)
--GO



--CREATE TABLE RolesInGroupRole(
--	GroupRoleId UNIQUEIDENTIFIER NOT NULL
--	,RoleId UNIQUEIDENTIFIER NOT NULL
--	,[Status] TINYINT DEFAULT 0

--	,ModifiedDate DATETIME
--	,CreatedDate DATETIME

--    ,CONSTRAINT PK_RolesInGroupRole PRIMARY KEY (GroupRoleId, RoleId)
--    ,CONSTRAINT FK_RolesInGroupRole_Role FOREIGN KEY (RoleId) REFERENCES Roles (Id)
--    ,CONSTRAINT FK_RolesInGroupRole_Group FOREIGN KEY (GroupRoleId) REFERENCES GroupRole (Id)
--)
--GO



-----========= GROUPS ===========---
--CREATE PROCEDURE pro_GroupRole_Verify
--@Name NVARCHAR (256)
--AS BEGIN
--	SELECT * FROM GroupRole WHERE LOWER([Name]) = LOWER(@Name)
--END
--GO

--CREATE PROCEDURE pro_GroupRole_Insert
--@CreatedBy		UNIQUEIDENTIFIER
--,@Id			UNIQUEIDENTIFIER
--,@Name			NVARCHAR (256)
--,@Description	NVARCHAR (1000)
--,@CreatedDate	DATETIME
--AS BEGIN
--SET XACT_ABORT ON
--BEGIN TRAN
--	DECLARE @Keys nvarchar(2000), @ObjectId varchar(128);

--	INSERT INTO GroupRole(Id, [Name], [Description], ModifiedDate, CreatedDate)
--			VALUES (@Id, @Name, @Description, @CreatedDate, @CreatedDate)

--	SET @Keys = '_GroupId:' + CAST(@Id AS nvarchar(128))
--				+ '_Name:' + @Name
--				+ '_Description:' + @Description

--	SET @ObjectId = CAST(@Id AS varchar(128))
--	EXEC pro_DataLog_Insert
--		@TableName		= 'GROUP'
--		,@Operation		= 'INSERT'
--		,@ObjectId		= @ObjectId
--		,@Content		= @Keys
--		,@UserId		= @CreatedBy
--		,@CreatedDate	= @CreatedDate
--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_GroupRole_Update
--@CreatedBy UNIQUEIDENTIFIER
--,@Id UNIQUEIDENTIFIER
--,@Name NVARCHAR (256)
--,@Description NVARCHAR (1000)
--,@Status TINYINT
--,@ModifiedDate	DATETIME
--AS BEGIN		
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(2000), @ObjectId varchar(128);

--		UPDATE GroupRole SET [Name] = @Name
--						,[Description] = @Description
--						,[Status] = @Status
--						,ModifiedDate = @ModifiedDate					
--				WHERE Id = @Id

--		SET @Keys = '_GroupId:' + CAST(@Id AS nvarchar(128))
--					+ '_Name:' + @Name
--					+ '_Description:' + @Description

--		SET @ObjectId = CAST(@Id AS varchar(128))
--		EXEC pro_DataLog_Insert
--			@TableName		= 'GROUP'
--			,@Operation		= 'UPDATE'
--			,@ObjectId		= @ObjectId
--			,@Content		= @Keys
--			,@UserId		= @CreatedBy
--			,@CreatedDate	= @ModifiedDate
--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_GroupRole_Delete
--@CreatedBy		UNIQUEIDENTIFIER
--,@Id			UNIQUEIDENTIFIER
--,@CreatedDate	DATETIME
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(1000), @ObjectId varchar(128);

--		DELETE FROM UsersInGroupRole WHERE GroupRoleId = @Id
--		DELETE FROM RolesInGroupRole WHERE GroupRoleId = @Id
--		DELETE FROM GroupRole WHERE Id = @Id


--	SET @ObjectId = CAST(@Id AS varchar(128))
--	EXEC pro_DataLog_Insert
--		@TableName		= 'GROUP'
--		,@Operation		= 'DELETE'
--		,@ObjectId		= @ObjectId
--		,@Content		= @Keys
--		,@UserId		= @CreatedBy
--		,@CreatedDate	= @CreatedDate
--	COMMIT
--END
--GO


--CREATE PROCEDURE pro_GroupRole_FindGroupsByUserId
--@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT g.* FROM GroupRole g
--				INNER JOIN UsersInGroupRole ug ON g.Id = ug.GroupRoleId
--	 WHERE ug.UserId = @UserId	
--END
--GO

--CREATE PROCEDURE pro_GroupRole_FindById
--@Id UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT g.* FROM GroupRole g WHERE g.Id = @Id	
--END
--GO


--CREATE PROCEDURE pro_GroupRole_FindBy
--@Keyword nvarchar(256)
--,@Status TINYINT
--,@BeginRow int
--,@NumRows int
--AS BEGIN
--		DECLARE @SQL nvarchar(max), @WHERE nvarchar(1000);

--	SET @SQL = 'WITH PRO_PAGIN AS (SELECT ROW_NUMBER() OVER (ORDER BY g.[Name] DESC) AS RowNum, COUNT(*) OVER () AS TotalRows, g.*
--						FROM GroupRole g '

--	SET @WHERE = ''

--	IF @Status != 254 
--		SET @WHERE = @WHERE + 'AND g.[Status] = @Status '

--	IF @Keyword IS NOT NULL AND LEN(@Keyword) > 0
--		SET @WHERE = @WHERE + 'AND CONTAINS(g.Name, @Keyword) '
	
--	IF @WHERE IS NOT NULL AND LEN(@WHERE) > 0
--		SET @SQL = @SQL + ' WHERE ' + RIGHT(@WHERE, LEN(@WHERE) - 3)

--	SET @SQL = @SQL + @WHERE  + ' ) SELECT pa.* FROM PRO_PAGIN pa '

--	IF @NumRows != 0
--		SET @SQL = @SQL + 'WHERE RowNum BETWEEN @BeginRow AND @NumRows ORDER BY RowNum ASC '

--	EXEC sp_executesql @SQL 
--	, N'@Keyword nvarchar(250),
--		@Status TINYINT,
--		@BeginRow int,
--		@NumRows int',
--		@Keyword,
--		@Status, 
--		@BeginRow,
--		@NumRows
--END
--GO

-----========= USER GROUP ===========---

--CREATE PROCEDURE pro_UsersInGroupRole_Insert
--@ListGroup list_guid_table READONLY
--,@UserId UNIQUEIDENTIFIER
--,@CreatedDate DATETIME
--AS BEGIN
--SET XACT_ABORT ON
--BEGIN TRAN
--	DELETE FROM UsersInGroupRole WHERE UserId = @UserId

--	INSERT INTO UsersInGroupRole(GroupRoleId, UserId, CreatedDate, ModifiedDate) SELECT Id, @UserId, @CreatedDate, @CreatedDate FROM @ListGroup
--COMMIT
--END
--GO


--CREATE PROCEDURE pro_UsersInGroupRole_FindUsersByGroupId
--@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT U.UserName, U.Email, ug.UserId, ug.GroupRoleId
--			 FROM GroupRole g
--				INNER JOIN UsersInGroupRole ug ON g.Id = ug.GroupRoleId
--				INNER JOIN Users u ON ug.UserId = u.Id
--	 WHERE ug.GroupRoleId = @GroupId
		
--END
--GO


--CREATE PROCEDURE pro_UsersInGroupRole_RemoveAllUserGroupsByUserId
--@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM UsersInGroupRole WHERE UserId = @UserId
--END
--GO

-----======= ROLE ========----
--CREATE PROCEDURE pro_RolesInGroupRole_Insert
--@ListRole list_guid_table READONLY
--,@GroupId UNIQUEIDENTIFIER
--,@CreatedDate DATETIME
--AS BEGIN
--SET XACT_ABORT ON
--BEGIN TRAN
--	DELETE FROM RolesInGroupRole WHERE GroupRoleId = @GroupId

--	INSERT INTO RolesInGroupRole(RoleId, GroupRoleId, CreatedDate, ModifiedDate) SELECT id, @GroupId, @CreatedDate, @CreatedDate FROM @ListRole
--COMMIT
--END
--GO

--CREATE PROCEDURE pro_RolesInGroupRole_GetListRoleByGroupId
--@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT r.Id AS RoleId, r.[Name] AS RoleName FROM Roles r INNER JOIN RolesInGroupRole rg ON r.Id = rg.RoleId AND rg.GroupRoleId = @GroupId
--END
--GO

--CREATE PROCEDURE pro_RolesInGroupRole_RemoveAllRolesInGroup
--@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM RolesInGroupRole WHERE GroupRoleId = @GroupId
--END
--GO