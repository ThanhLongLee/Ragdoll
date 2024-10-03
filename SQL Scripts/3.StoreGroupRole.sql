
-----========= GROUPS ===========---
--CREATE PROCEDURE pro_Group_Verify
--@Name			NVARCHAR (256)
--AS BEGIN
--	SELECT * FROM Groups WHERE [Name] = @Name
--END
--GO

--CREATE PROCEDURE pro_Group_Insert
--@CreatedBy UNIQUEIDENTIFIER
--,@GroupId UNIQUEIDENTIFIER
--,@Name			NVARCHAR (256)
--,@Description	NVARCHAR (256)
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(1000), @ObjectId varchar(128);

--		INSERT INTO Groups(Id, [Name], [Description])
--			VALUES (@GroupId, @Name, @Description)

--		SET @Keys = '_GroupId:' + CAST(@GroupId AS nvarchar(256))
--					+ '_Name:' + @Name
--					+ '_Description:' + @Description

--	SET @ObjectId = CAST(@GroupId AS varchar(128))
--	EXEC pro_DataLog_Insert
--		@TableName		= 'GROUP'
--		,@Operation		= 'INSERT'
--		,@ObjectId		= @ObjectId
--		,@Content		= @Keys
--		,@UserId		= @CreatedBy

--			SELECT * FROM Groups WHERE Id = @GroupId
--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_Group_Delete
--@CreatedBy UNIQUEIDENTIFIER
--,@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(1000), @ObjectId varchar(128);

--		DELETE FROM UserGroups WHERE GroupId = @GroupId
--		DELETE FROM RoleGroups WHERE GroupId = @GroupId
--		DELETE FROM Groups WHERE Id = @GroupId


--	SET @ObjectId = CAST(@GroupId AS varchar(128))
--	EXEC pro_DataLog_Insert
--		@TableName		= 'GROUP'
--		,@Operation		= 'DELETE'
--		,@ObjectId		= @ObjectId
--		,@Content		= @Keys
--		,@UserId		= @CreatedBy

--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_Groups_Update
--@CreatedBy UNIQUEIDENTIFIER
--,@GroupId UNIQUEIDENTIFIER
--,@Name NVARCHAR (256)
--,@Description NVARCHAR (256)
--AS BEGIN		
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(1000), @ObjectId varchar(128);

--		UPDATE Groups SET Name = @Name, [Description] = @Description WHERE Id = @GroupId

--		SET @Keys = '_GroupId:' + CAST(@GroupId AS nvarchar(256))
--					+ '_Name:' + @Name
--					+ '_Description:' + @Description

--		SET @ObjectId = CAST(@GroupId AS varchar(128))
--		EXEC pro_DataLog_Insert
--			@TableName		= 'GROUP'
--			,@Operation		= 'UPDATE'
--			,@ObjectId		= @ObjectId
--			,@Content		= @Keys
--			,@UserId		= @CreatedBy
--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_Group_GetGroupsByUserId
--@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT g.* FROM Groups g
--				INNER JOIN UserGroups ug ON g.Id = ug.GroupId
--	 WHERE ug.UserId = @UserId
		
--END
--GO

--CREATE PROCEDURE pro_User_GetUsersByGroupId
--@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate
--			 FROM Groups g
--				INNER JOIN UserGroups ug ON g.Id = ug.GroupId
--				INNER JOIN Users u ON ug.UserId = u.Id
--	 WHERE ug.GroupId = @GroupId
		
--END
--GO

--CREATE PROCEDURE pro_Group_GetDetail
--@Id UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT g.* FROM Groups g WHERE g.Id = @Id	
--END
--GO



--CREATE PROCEDURE pro_Group_GetAll
--AS BEGIN
--	SELECT * FROM Groups
--END
--GO

--CREATE PROCEDURE pro_Group_FindBy
--@Keyword nvarchar(256)
--,@BeginRow int
--,@NumRows int
--AS BEGIN
--		DECLARE @SQL nvarchar(max), @WHERE nvarchar(1000);

--	SET @SQL = 'WITH PRO_PAGIN AS (SELECT ROW_NUMBER() OVER (ORDER BY g.Name DESC) AS RowNum, COUNT(*) OVER () AS TotalRows, g.*
--						FROM Groups g '

--	SET @WHERE = ''

--	IF @Keyword IS NOT NULL AND LEN(@Keyword) > 0
--		SET @WHERE = @WHERE + 'AND CONTAINS(g.Name, @Keyword) '
	
--	IF @WHERE IS NOT NULL AND LEN(@WHERE) > 0
--		SET @SQL = @SQL + ' WHERE ' + RIGHT(@WHERE, LEN(@WHERE) - 3)

--	SET @SQL = @SQL + @WHERE  + ' ) SELECT pa.* FROM PRO_PAGIN pa '

--	IF @NumRows != 0
--		SET @SQL = @SQL + 'WHERE RowNum BETWEEN @BeginRow AND @NumRows ORDER BY RowNum ASC '

--	EXEC sp_executesql @SQL 
--	, N'@Keyword nvarchar(250),
--		@BeginRow int,
--		@NumRows int',
--		@Keyword,
--		@BeginRow,
--		@NumRows
--END
--GO

-----========= USER GROUP ===========---

--CREATE PROCEDURE pro_User_AddUserToGroups
--@ListGroup list_guid_table READONLY
--,@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM UserGroups WHERE UserId = @UserId

--	INSERT INTO UserGroups(GroupId, UserId) SELECT Id, @UserId FROM @ListGroup
--END
--GO

-----======= ROLE ========----
--CREATE PROCEDURE pro_Role_AddRolesToGroup
--@ListRole list_guid_table READONLY
--,@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM RoleGroups WHERE GroupId = @GroupId

--	INSERT INTO RoleGroups(RoleId, GroupId) SELECT id, @GroupId FROM @ListRole
--END
--GO

--CREATE PROCEDURE pro_Role_GetListRoleByGroupId
--@GroupId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT * FROM Roles r INNER JOIN RoleGroups rg ON r.Id = rg.RoleId AND rg.GroupId = @GroupId
--END
--GO

-----======= USER ========----
--CREATE PROCEDURE pro_User_RemoveAllUserGroupsByUserId
--@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM UserGroups WHERE UserId = @UserId
--END
--GO