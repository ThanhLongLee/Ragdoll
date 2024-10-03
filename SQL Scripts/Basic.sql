
---- FullTextSearch
--CREATE FULLTEXT CATALOG [CatalogFTS] WITH ACCENT_SENSITIVITY = OFF
--GO

---- 
--CREATE TYPE [list_guid_table] AS TABLE(
--	[id] [uniqueidentifier] NULL
--)
--GO
--CREATE TYPE [list_id_table] AS TABLE(
--	[id] [int] NULL
--)
--GO
--CREATE TYPE [list_strId_table] AS TABLE(
--	[Id] [varchar](250) NULL
--)
--GO
--CREATE TYPE [list_type_table] AS TABLE(
--	[type] [tinyint] NULL
--)
--GO


--CREATE TYPE [list_sort_index] AS TABLE(
--	Id INT
--	,[Index] INT
--)
--GO

----======== DATE LOG =============-

--CREATE TABLE DataLog(
--	[Id] [bigint] IDENTITY(1,1) NOT NULL,
--	[TableName] [varchar](50) NULL,
--	[Operation] [varchar](50) NULL,
--	[ObjectId] [varchar](128) NULL,
--	[Content] nvarchar(MAX) NULL,
--	[UserId] [uniqueidentifier] NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,

--    CONSTRAINT PK_DataLog PRIMARY KEY (Id)
--)
--GO


--CREATE PROCEDURE [dbo].[pro_DataLog_Insert]
--@TableName		VARCHAR(50)
--,@Operation		VARCHAR(50)
--,@ObjectId		VARCHAR(128)
--,@Content		nvarchar(MAX)
--,@UserId		UNIQUEIDENTIFIER
--,@CreatedDate	DATETIME
--AS BEGIN
--	INSERT INTO DataLog(TableName, Operation, ObjectId, Content, UserId, CreatedDate)
--	VALUES (@TableName, @Operation, @ObjectId, @Content, @UserId, @CreatedDate);
--END
--GO



----======== ERROR LOG =============-

--CREATE TABLE ErrorLog (
--	[Id] [bigint] IDENTITY(1,1) NOT NULL,
--	[Title] nvarchar(MAX) NOT NULL,
--	[Content] nvarchar(MAX) NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,

--    CONSTRAINT PK_ErrorLog PRIMARY KEY (Id)
--)
--GO


--CREATE PROCEDURE pro_ErrorLog_Insert
--@Title nvarchar(MAX)
--, @Content nvarchar(MAX)
--, @CreatedDate datetime
--AS BEGIN
--	INSERT INTO ErrorLog(Title, Content, CreatedDate)
--	VALUES (@Title, @Content, @CreatedDate);
--END
--GO