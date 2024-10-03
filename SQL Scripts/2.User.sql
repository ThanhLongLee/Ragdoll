
--CREATE TABLE Users(
--  Id						UNIQUEIDENTIFIER NOT NULL,
--  UserName				NVARCHAR (256)   NOT NULL,
--  Email					NVARCHAR (256)   NULL,
--  EmailConfirmed			BIT              NOT NULL,
--  PasswordHash			NVARCHAR (500)   NULL,
--  PhoneNumber				NVARCHAR (500)   NULL,
--  PhoneNumberConfirmed	BIT              NOT NULL,
--  SecurityStamp			NVARCHAR (500)   NULL,
--  TwoFactorEnabled		BIT              NOT NULL,
--  LockoutEndDateUtc		DATETIME         NULL,
--  LockoutEnabled			BIT              NOT NULL,
--  AccessFailedCount		INT              NOT NULL,
--  CreatedDate				DATETIME,
--	ModifyDate				DATETIME,

--    CONSTRAINT PK_User PRIMARY KEY (Id)
--)
--GO

--CREATE TABLE UserProfiles(
--	UserId					UNIQUEIDENTIFIER NOT NULL,
--	IdentityCode			BIGINT IDENTITY(1,1),
--	FullName				NVARCHAR (256),
--	UserType				TINYINT DEFAULT 0,
--	UserRole				TINYINT DEFAULT 0,
--	Note					NVARCHAR(MAX) DEFAULT '',
--	[Status]			    TINYINT DEFAULT 0,

--	CONSTRAINT PK_UserProfiles PRIMARY KEY (UserId),
--    CONSTRAINT FK_UserProfiles_User FOREIGN KEY (UserId) REFERENCES Users (Id)
--)
--GO

---- Tên chung của role: quản lý tài khoản,....
--CREATE TABLE ParentRole (
--    Id				UNIQUEIDENTIFIER NOT NULL,
--    [Name]			NVARCHAR (256)   NOT NULL,
--	[Description]	NVARCHAR (500),
--	[Index]			INT
--    CONSTRAINT PK_ParentRole PRIMARY KEY (Id)
--)
--GO


---- Tên con: xem, thêm, xóa, sửa tài khoản
--CREATE TABLE Roles (
--    Id				UNIQUEIDENTIFIER NOT NULL,
--    [Name]			NVARCHAR (256)   NOT NULL,
--	[Description]	NVARCHAR (256),
--	ParentId UNIQUEIDENTIFIER NOT NULL,

--    CONSTRAINT PK_Role PRIMARY KEY (Id)
--	,CONSTRAINT fk_Roles_RoleParent FOREIGN KEY (ParentId) REFERENCES ParentRole(Id)
--)
--GO

--CREATE TABLE UserRoles (
--    UserId UNIQUEIDENTIFIER NOT NULL,
--    RoleId UNIQUEIDENTIFIER NOT NULL,

--    CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
--    CONSTRAINT FK_UserRole_Role_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id),
--    CONSTRAINT FK_UserRole_User_UserId FOREIGN KEY (UserId) REFERENCES Users (Id)
--)
--GO

--CREATE TABLE UserLogins (
--    LoginProvider NVARCHAR (128)   NOT NULL,
--    ProviderKey   NVARCHAR (128)   NOT NULL,
--    UserId        UNIQUEIDENTIFIER NOT NULL,

--    CONSTRAINT PK_UserLogin PRIMARY KEY (LoginProvider, ProviderKey, UserId),
--    CONSTRAINT FK_UserLogin_User_UserId FOREIGN KEY (UserId) REFERENCES Users (Id)
--)
--GO

--CREATE TABLE UserClaims (
--    Id		 UNIQUEIDENTIFIER NOT NULL,
--    UserId     UNIQUEIDENTIFIER NOT NULL,
--    ClaimType  NVARCHAR (500)   NULL,
--    ClaimValue NVARCHAR (500)   NULL,

--    CONSTRAINT PK_UserClaim PRIMARY KEY (Id),
--    CONSTRAINT FK_UserClaim_User_UserId FOREIGN KEY (UserId) REFERENCES Users (Id)
--)
--GO


--/*====================================================
--					PROCEDURE
--=====================================================*/

----========= USER ===========---

ALTER PROCEDURE pro_UserProfiles_Create
@UserId					UNIQUEIDENTIFIER
,@FullName				NVARCHAR (256)
,@UserType			    TINYINT
,@UserRole			    TINYINT
AS BEGIN

		INSERT INTO UserProfiles(UserId, FullName, UserType, UserRole)
		VALUES(@UserId, @FullName, @UserType, @UserRole)
END
GO

--CREATE PROCEDURE pro_User_Create
--@CreatedBy UNIQUEIDENTIFIER
--,@Id						UNIQUEIDENTIFIER
--,@UserName				NVARCHAR (256)   
--,@Email					NVARCHAR (256)   
--,@EmailConfirmed		BIT              
--,@PasswordHash			NVARCHAR (500)   
--,@PhoneNumber			NVARCHAR (500)   
--,@PhoneNumberConfirmed	BIT              
--,@SecurityStamp			NVARCHAR (500)   
--,@TwoFactorEnabled		BIT              
--,@LockoutEndDateUtc		DATETIME         
--,@LockoutEnabled		BIT              
--,@AccessFailedCount		INT

--,@FullName				NVARCHAR (256)
--,@UserType			    TINYINT
--,@UserRole			    TINYINT
--,@CreatedDate			DATETIME
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @ObjectId VARCHAR(128), @Keys nvarchar(1000)

--		INSERT INTO Users(Id, UserName,	Email, EmailConfirmed, PasswordHash, PhoneNumber, PhoneNumberConfirmed
--						  ,SecurityStamp,TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, CreatedDate, ModifyDate)	
--		VALUES(@Id, @UserName, @Email, @EmailConfirmed, @PasswordHash, @PhoneNumber, @PhoneNumberConfirmed
--						  ,@SecurityStamp, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @CreatedDate, @CreatedDate)
		
--		EXEC pro_UserProfiles_Create
--			@UserId		= @Id		
--			,@FullName  = @FullName			
--			,@UserType	= @UserType
--			,@UserRole =  @UserRole
			
--		SET @Keys = '_UserId:' + CAST(@Id AS nvarchar(256))
--					+ '_UserName:' + @UserName
--					+ '_FullName:' + @FullName
--					+ '_UserType:' + + CAST(@UserType AS nvarchar(5))

--		SET @ObjectId = CAST(@Id AS VARCHAR(128));
--		EXEC pro_DataLog_Insert
--				@TableName		= 'USER'
--				, @Operation	= 'CREATE'
--				, @ObjectId		= @ObjectId
--				, @Content		= @Keys
--				, @UserId		= @CreatedBy
--				,@CreatedDate	= @CreatedDate

--	COMMIT
--END
--GO


--CREATE PROCEDURE pro_User_Delete
--@CreatedBy UNIQUEIDENTIFIER
--,@UserId	UNIQUEIDENTIFIER
--,@CreatedDate DATETIME
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @ObjectId VARCHAR(128), @Keys nvarchar(1000)

--		DELETE FROM UserProfiles WHERE UserId = @UserId
--		DELETE FROM UserS WHERE Id = @UserId
					
--		SET @Keys = '_UserId:' + CAST(@UserId AS nvarchar(256))

--		SET @ObjectId = CAST(@UserId AS VARCHAR(128));
--		EXEC pro_DataLog_Insert
--				@TableName		= 'USER'
--				, @Operation	= 'DELETE'
--				, @ObjectId		= @ObjectId
--				, @Content		= @Keys
--				, @UserId		= @CreatedBy
--				,@CreatedDate = @CreatedDate

--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_User_Update
--@CreatedBy UNIQUEIDENTIFIER
--,@Id					UNIQUEIDENTIFIER
--,@UserName				NVARCHAR (256)   
--,@Email					NVARCHAR (256)   
--,@EmailConfirmed		BIT              
--,@PasswordHash			NVARCHAR (500)   
--,@PhoneNumber			NVARCHAR (500)   
--,@PhoneNumberConfirmed	BIT              
--,@SecurityStamp			NVARCHAR (500)   
--,@TwoFactorEnabled		BIT              
--,@LockoutEndDateUtc		DATETIME         
--,@LockoutEnabled		BIT              
--,@AccessFailedCount		INT

--,@FullName				NVARCHAR (256)
--,@Note					NVARCHAR (MAX)
--,@CreatedDate DATETIME
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @ObjectId VARCHAR(128), @Keys nvarchar(1000)

--		UPDATE Users SET UserName = @UserName
--						,Email = @Email
--						,EmailConfirmed = @EmailConfirmed
--						,PasswordHash = @PasswordHash
--						,PhoneNumber	= @PhoneNumber
--						,PhoneNumberConfirmed = @PhoneNumberConfirmed
--						,SecurityStamp = @SecurityStamp
--						,TwoFactorEnabled = @TwoFactorEnabled
--						,LockoutEndDateUtc = @LockoutEndDateUtc
--						,LockoutEnabled = @LockoutEnabled
--						,AccessFailedCount = @AccessFailedCount
--						,ModifyDate = @CreatedDate
--				WHERE Id = @Id
		
--		UPDATE UserProfiles SET FullName = @FullName
--								,Note = @Note
--							WHERE UserId = @Id

--		SET @Keys = '@UserId:' + CAST(@Id AS nvarchar(256))
--					+ '@UserName:' + @UserName
--					+ '@FullName:' + @FullName

--		SET @ObjectId = CAST(@Id AS VARCHAR(128));
--		EXEC pro_DataLog_Insert
--				@TableName		= 'USER'
--				, @Operation	= 'UPDATE'
--				, @ObjectId		= @ObjectId
--				, @Content		= @Keys
--				, @UserId		= @CreatedBy
--				,@CreatedDate = @CreatedDate
--	COMMIT
--END
--GO


--CREATE PROCEDURE pro_User_UpdateStatus
--@CreatedBy UNIQUEIDENTIFIER
--,@UserId	UNIQUEIDENTIFIER
--,@Status	TINYINT
--,@CreatedDate DATETIME
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		DECLARE @Keys nvarchar(1000), @ObjectId VARCHAR(128);

--		UPDATE UserProfiles SET [Status] = @Status WHERE UserId = @UserId
--		UPDATE Users SET ModifyDate = @CreatedDate WHERE Id = @UserId

--		SET @Keys = '@UserId:' + CAST(@UserId AS nvarchar(256))
--					+ '@Status:' + CAST(@Status AS nvarchar(5))

--		SET @ObjectId = CAST(@UserId AS VARCHAR(128));
--		EXEC pro_DataLog_Insert
--				@TableName		= 'USER'
--				, @Operation	= 'UPDATE'
--				, @ObjectId		= @ObjectId
--				, @Content		= @Keys
--				, @UserId		= @CreatedBy
--				,@CreatedDate = @CreatedDate


--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_Uset_UpdateSecurityStamp
--@UserId UNIQUEIDENTIFIER
--,@NewSecurityStamp NVARCHAR(500)
--,@CreatedDate DATETIME
--AS BEGIN
--	UPDATE Users SET SecurityStamp = @NewSecurityStamp
--					,ModifyDate = @CreatedDate
--	 WHERE Id = @UserId
--END
--GO


--CREATE PROCEDURE pro_User_FindByName
--@UserName nvarchar (256)
--AS BEGIN

--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate, UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE U.UserName = @UserName AND UP.[Status] != 250
--END
--GO



--CREATE PROCEDURE pro_User_FindById
--@UserId	UNIQUEIDENTIFIER
--AS BEGIN
	
--		SELECT U.Id, U.UserName, UP.FullName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate,
--			UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE U.Id = @UserId AND UP.[Status] != 250

--END
--GO

--CREATE PROCEDURE pro_User_FindByEmail
--@Email nvarchar (256)
--AS BEGIN

--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate, UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE U.Email = @Email AND UP.[Status] != 250
--END
--GO

--CREATE PROCEDURE pro_User_GetAll
--AS BEGIN
--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate, UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE UP.[Status] != 250
--END
--GO

--CREATE PROCEDURE pro_User_FindBy
--@Keyword nvarchar(256)
--,@UserRoles list_type_table READONLY
--,@Status tinyint
--,@StartDate		DATETIME
--,@EndDate		DATETIME
--,@SortByName		BIT
--,@SortByDate		BIT
--,@SortDesc			BIT
--,@BeginRow int
--,@NumRows int
--AS BEGIN
--	DECLARE @SQL nvarchar(max), @WHERE nvarchar(2000), @OrderBy nvarchar(500), @OrderExpression varchar(10);
		
--	SET @OrderExpression = 'ASC'

--	IF @SortDesc > 0
--		SET @OrderExpression = 'DESC'
	
--	SET @OrderBy = 'u.CreatedDate'

--	IF @SortByDate > 0
--		SET @OrderBy = 'u.CreatedDate'

--	IF @SortByName > 0
--		SET @OrderBy = 'up.FullName'

--	SET @SQL = 'WITH PRO_PAGIN AS (SELECT ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ' ' + @OrderExpression + ') AS RowNum, COUNT(*) OVER () AS TotalRows,
--						U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--						U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, ''+00:00'') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate
--						,up.FullName, up.UserType, up.[Status], up.Note, up.UserRole
--						FROM Users u
--						INNER JOIN UserProfiles up ON u.Id = up.UserId '

--	SET @WHERE = ''

--	IF EXISTS (SELECT * FROM @UserRoles)
--		SET @WHERE = @WHERE + 'AND up.UserRole IN (SELECT [type] FROM @UserRoles) '

--	IF @Status != 254
--		SET @WHERE = @WHERE + 'AND up.[Status] = @Status '

--	IF @Keyword IS NOT NULL AND LEN(@Keyword) > 0
--		SET @WHERE = @WHERE + 'AND (CONTAINS(up.FullName, @Keyword) OR CONTAINS((u.UserName, u.PhoneNumber, u.Email), @Keyword))'
	
--	IF @StartDate IS NOT NULL AND LEN(@StartDate) > 0 
--		IF DATEDIFF(DAY, @StartDate, @EndDate) = 0
--			SET @WHERE = @WHERE + 'AND DATEDIFF(DAY, u.CreatedDate, @StartDate) = 0 ';
--		ELSE
--			SET @WHERE = @WHERE + 'AND u.CreatedDate BETWEEN @StartDate AND @EndDate ';

--	IF @WHERE IS NOT NULL AND LEN(@WHERE) > 0
--		SET @SQL = @SQL + ' WHERE ' + RIGHT(@WHERE, LEN(@WHERE) - 3)

--	SET @SQL = @SQL + ' ) SELECT pa.* FROM PRO_PAGIN pa '

--	IF @NumRows != 0
--		SET @SQL = @SQL + 'WHERE RowNum BETWEEN @BeginRow AND @NumRows ORDER BY RowNum ASC '

--	EXEC sp_executesql @SQL 
--	, N'@Keyword nvarchar(250)	
--		,@UserRoles list_type_table READONLY
--		,@Status tinyint
--		,@StartDate		DATETIME
--		,@EndDate		DATETIME
--		,@SortByName		BIT
--		,@SortByDate		BIT
--		,@SortDesc			BIT
--		,@BeginRow int
--		,@NumRows int',
--		@Keyword
--		,@UserRoles
--		,@Status
--		,@StartDate		
--		,@EndDate		
--		,@SortByName		
--		,@SortByDate			
--		,@SortDesc			
--		,@BeginRow
--		,@NumRows
--END
--GO

--CREATE PROCEDURE pro_User_VerifyEmail
--@Email varchar(100)
--AS BEGIN
--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate,
--			UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE U.Email = @Email
--END
--GO

--CREATE PROCEDURE pro_User_VerifyPhoneNumber
--@PhoneNumber nvarchar(100)
--AS BEGIN
--	SELECT U.Id, U.UserName, U.Email, U.EmailConfirmed, U.PasswordHash, U.PhoneNumber, U.PhoneNumberConfirmed, U. SecurityStamp,
--			U.TwoFactorEnabled, U.LockoutEnabled, TODATETIMEOFFSET(U.LockoutEndDateUtc, '+00:00') AS LockoutEndDateUtc, U.AccessFailedCount, U.CreatedDate, U.ModifyDate,
--			UP.*
--			FROM Users U INNER JOIN UserProfiles UP ON U.Id = UP.UserId 
--			WHERE U.PhoneNumber = @PhoneNumber
--END
--GO


----========= USER ROLE ===========---
--CREATE PROCEDURE pro_UserRole_AddToRole
--@UserId UNIQUEIDENTIFIER
--,@RoleName nvarchar(256)
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--		------ get role id
--		DECLARE @RoleId uniqueidentifier = (SELECT Id FROM Roles WHERE Name=  @RoleName)

--		--- insert, if not exist
--		INSERT INTO UserRoles (UserId, RoleId)
--		SELECT U.Id, @RoleId
--				FROM Users U 
--					LEFT JOIN UserRoles UR ON U.Id = UR.UserId AND UR.RoleId = @RoleId
--				WHERE U.Id = @UserId AND UR.UserId IS NULL
--	COMMIT
--END
--GO


--CREATE PROCEDURE pro_UserRole_AddFromListRole
--@ListRole list_guid_table READONLY
--,@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	INSERT INTO UserRoles (RoleId, UserId) SELECT id, @UserId FROM @ListRole
--END
--GO



--CREATE PROCEDURE pro_UserRole_GetRoles
--@UserId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT R.Name AS RoleName FROM Roles R
--					INNER JOIN UserRoles UR ON R.Id = UR.RoleId AND UR.UserId = @UserId
--END
--GO

--CREATE PROCEDURE pro_UserRole_RemoveFromRole
--@UserId UNIQUEIDENTIFIER
--,@RoleName nvarchar(256)
--AS BEGIN
--	 --get role id
--    DECLARE @RoleId uniqueidentifier = (SELECT Id FROM Roles WHERE Name = @RoleName)

--     --delete
--    DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId
	
--END
--GO


--CREATE PROCEDURE pro_UserRole_RemoveAllUserRolesByUserId
-- @UserId uniqueidentifier
--AS BEGIN
--	DELETE FROM UserRoles WHERE UserId = @UserId
--END
--GO

----========= USER LOGIN ===========---
--CREATE PROCEDURE pro_UserLogin_AddLogin
--@LoginProvider	nvarchar (128)  
--,@ProviderKey	nvarchar (128)  
--,@UserId		UNIQUEIDENTIFIER
--AS BEGIN
	
--	INSERT INTO UserLogins (LoginProvider, ProviderKey, UserId)
--     VALUES (@LoginProvider, @ProviderKey, @UserId)

--END
--GO

--CREATE PROCEDURE pro_UserLogin_Find
--@LoginProvider	nvarchar (128)  
--,@ProviderKey	nvarchar (128)  
--AS BEGIN
--	SELECT UserId FROM UserLogins
--		WHERE LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey
--END
--GO

--CREATE PROCEDURE pro_UserLogin_GetLogins
--@UserId	UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT * FROM UserLogins WHERE UserId = @UserId
--END
--GO

--CREATE PROCEDURE pro_UserLogin_RemoveLogin
--@LoginProvider	nvarchar (128)  
--,@ProviderKey	nvarchar (128)  
--,@UserId		UNIQUEIDENTIFIER
--AS BEGIN
--	DELETE FROM UserLogins
--			 WHERE UserId= @UserId AND LoginProvider=@LoginProvider AND ProviderKey=@ProviderKey
--END
--GO

----========= ROLES ===========---
--CREATE PROCEDURE pro_Roles_Create
--@Id				UNIQUEIDENTIFIER
--,@Name			NVARCHAR (256)
--,@Description	NVARCHAR (256)
--AS BEGIN
--	SET XACT_ABORT ON
--	BEGIN TRAN
--			INSERT INTO Roles(Id, Name, [Description])
--			VALUES (@Id, @Name, @Description)

--	COMMIT
--END
--GO

--CREATE PROCEDURE pro_Roles_Delete
--@RoleId	UNIQUEIDENTIFIER
--AS BEGIN
--		DELETE FROM Roles WHERE Id = @RoleId
--END
--GO

--CREATE PROCEDURE pro_Roles_FindById
--@RoleId	UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT * FROM Roles WHERE Id = @RoleId
--END
--GO

--CREATE PROCEDURE pro_Roles_FindByName
--@Name NVARCHAR (256)
--AS BEGIN
--	SELECT * FROM Roles WHERE Name = @Name
--END
--GO

--CREATE PROCEDURE pro_Roles_Update
--@RoleId	UNIQUEIDENTIFIER
--,@Name NVARCHAR (256)
--,@Description	NVARCHAR (256)
--AS BEGIN		
--	UPDATE Roles SET [Name] = @Name, [Description] = @Description WHERE Id = @RoleId
--END
--GO

--CREATE PROCEDURE pro_Roles_GetAll
--AS BEGIN
--	SELECT * FROM Roles
--END
--GO

--CREATE PROCEDURE pro_Roles_GetAllParentRole
--AS BEGIN
--	SELECT * FROM ParentRole ORDER BY [Index]
--END
--GO

--CREATE PROCEDURE pro_Roles_FindByParentId
--@ParentId UNIQUEIDENTIFIER
--AS BEGIN
--	SELECT * FROM Roles WHERE ParentId = @ParentId
--END
--GO


--CREATE PROCEDURE pro_Roles_GetAllWithParent
--AS BEGIN
--	SELECT r.*, pr.[Name] AS ParentName, pr.Id AS ParentId FROM ParentRole pr INNER JOIN Roles r ON pr.Id = r.ParentId ORDER BY r.ParentId 
--END
--GO



