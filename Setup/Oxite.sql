/*
Script created by SQL Compare version 7.1.0 from Red Gate Software Ltd at 2/13/2009 10:40:46 AM
Run this script on a blank database (make sure the database is selected first, or add a "use <db name>" statement)
to make it have the same schema as the Oxite database project.

An option line at the bottom of the script (commented out right now) can be used to create your root site.

Please back up your database before running this script
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Creating [dbo].[oxite_Tag]'
GO
CREATE TABLE [dbo].[oxite_Tag]
(
[ParentTagID] [uniqueidentifier] NOT NULL,
[TagID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_Tag_TagID] DEFAULT (newid()),
[TagName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Tag] on [dbo].[oxite_Tag]'
GO
ALTER TABLE [dbo].[oxite_Tag] ADD CONSTRAINT [PK_oxite_Tag] PRIMARY KEY CLUSTERED ([TagID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Site]'
GO
CREATE TABLE [dbo].[oxite_Site]
(
[SiteID] [uniqueidentifier] NOT NULL,
[SiteHost] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SiteName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SiteDisplayName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SiteDescription] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LanguageDefault] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[TimeZoneOffset] [float] NOT NULL,
[PageTitleSeparator] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FavIconUrl] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ScriptsPath] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CssPath] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommentStateDefault] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncludeOpenSearch] [bit] NOT NULL,
[AuthorAutoSubscribe] [bit] NOT NULL,
[PostEditTimeout] [smallint] NOT NULL,
[GravatarDefault] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SkinDefault] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServiceRetryCountDefault] [tinyint] NOT NULL CONSTRAINT [DF_oxite_Site_ServiceRetryCountDefault] DEFAULT ((3)),
[HasMultipleAreas] [bit] NOT NULL CONSTRAINT [DF_oxite_Site_HasMultipleAreas] DEFAULT ((0)),
[RouteUrlPrefix] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_oxite_Site_RouteUrlPrefix] DEFAULT (N''),
[CommentingDisabled] [bit] NOT NULL CONSTRAINT [DF_oxite_Site_AllowComments] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Site] on [dbo].[oxite_Site]'
GO
ALTER TABLE [dbo].[oxite_Site] ADD CONSTRAINT [PK_oxite_Site] PRIMARY KEY CLUSTERED ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Post]'
GO
CREATE TABLE [dbo].[oxite_Post]
(
[PostID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_Post_PostID] DEFAULT (newid()),
[CreatorUserID] [uniqueidentifier] NOT NULL,
[Title] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Body] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BodyShort] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[State] [tinyint] NOT NULL,
[Slug] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommentingDisabled] [bit] NOT NULL CONSTRAINT [DF_oxite_Post_AllowComments] DEFAULT ((0)),
[CreatedDate] [datetime] NOT NULL,
[ModifiedDate] [datetime] NOT NULL,
[PublishedDate] [datetime] NOT NULL,
[SearchBody] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Post] on [dbo].[oxite_Post]'
GO
ALTER TABLE [dbo].[oxite_Post] ADD CONSTRAINT [PK_oxite_Post] PRIMARY KEY CLUSTERED ([PostID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_oxite_Post] on [dbo].[oxite_Post]'
GO
CREATE NONCLUSTERED INDEX [IX_oxite_Post] ON [dbo].[oxite_Post] ([Slug])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_FileResource]'
GO
CREATE TABLE [dbo].[oxite_FileResource]
(
[SiteID] [uniqueidentifier] NOT NULL,
[FileResourceID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_FileResource_FileResourceID] DEFAULT (newid()),
[FileResourceName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatorUserID] [uniqueidentifier] NOT NULL,
[Data] [varbinary] (max) NULL,
[ContentType] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Path] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[State] [tinyint] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_FileResource] on [dbo].[oxite_FileResource]'
GO
ALTER TABLE [dbo].[oxite_FileResource] ADD CONSTRAINT [PK_oxite_FileResource] PRIMARY KEY CLUSTERED ([FileResourceID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Area]'
GO
CREATE TABLE [dbo].[oxite_Area]
(
[SiteID] [uniqueidentifier] NOT NULL,
[AreaID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_Area_AreaID] DEFAULT (newid()),
[AreaName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommentingDisabled] [bit] NOT NULL CONSTRAINT [DF_oxite_Area_AllowComments] DEFAULT ((0)),
[CreatedDate] [datetime] NOT NULL,
[ModifiedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Area] on [dbo].[oxite_Area]'
GO
ALTER TABLE [dbo].[oxite_Area] ADD CONSTRAINT [PK_oxite_Area] PRIMARY KEY CLUSTERED ([AreaID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_oxite_Area] on [dbo].[oxite_Area]'
GO
CREATE NONCLUSTERED INDEX [IX_oxite_Area] ON [dbo].[oxite_Area] ([AreaName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_User]'
GO
CREATE TABLE [dbo].[oxite_User]
(
[UserID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_User_UserID] DEFAULT (newid()),
[Username] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DisplayName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[HashedEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PasswordSalt] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DefaultLanguageID] [uniqueidentifier] NOT NULL,
[Status] [tinyint] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_User] on [dbo].[oxite_User]'
GO
ALTER TABLE [dbo].[oxite_User] ADD CONSTRAINT [PK_oxite_User] PRIMARY KEY CLUSTERED ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Trackback]'
GO
CREATE TABLE [dbo].[oxite_Trackback]
(
[PostID] [uniqueidentifier] NOT NULL,
[TrackbackID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_oxite_Trackback_TrackbackID] DEFAULT (newid()),
[Url] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Title] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Body] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Source] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BlogName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsTargetInSource] [bit] NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Trackback] on [dbo].[oxite_Trackback]'
GO
ALTER TABLE [dbo].[oxite_Trackback] ADD CONSTRAINT [PK_oxite_Trackback] PRIMARY KEY CLUSTERED ([TrackbackID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_AreaRoleRelationship]'
GO
CREATE TABLE [dbo].[oxite_AreaRoleRelationship]
(
[AreaID] [uniqueidentifier] NOT NULL,
[RoleID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_AreaRoleRelationship] on [dbo].[oxite_AreaRoleRelationship]'
GO
ALTER TABLE [dbo].[oxite_AreaRoleRelationship] ADD CONSTRAINT [PK_oxite_AreaRoleRelationship] PRIMARY KEY CLUSTERED ([AreaID], [RoleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Role]'
GO
CREATE TABLE [dbo].[oxite_Role]
(
[ParentRoleID] [uniqueidentifier] NOT NULL,
[RoleID] [uniqueidentifier] NOT NULL,
[RoleName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Role] on [dbo].[oxite_Role]'
GO
ALTER TABLE [dbo].[oxite_Role] ADD CONSTRAINT [PK_oxite_Role] PRIMARY KEY CLUSTERED ([RoleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Language]'
GO
CREATE TABLE [dbo].[oxite_Language]
(
[LanguageID] [uniqueidentifier] NOT NULL,
[LanguageName] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LanguageDisplayName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Language] on [dbo].[oxite_Language]'
GO
ALTER TABLE [dbo].[oxite_Language] ADD CONSTRAINT [PK_oxite_Language] PRIMARY KEY CLUSTERED ([LanguageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Comment]'
GO
CREATE TABLE [dbo].[oxite_Comment]
(
[PostID] [uniqueidentifier] NOT NULL,
[CommentID] [uniqueidentifier] NOT NULL,
[CreatorUserID] [uniqueidentifier] NOT NULL,
[LanguageID] [uniqueidentifier] NOT NULL,
[CreatorIP] [bigint] NOT NULL,
[UserAgent] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Body] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[State] [tinyint] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Comment] on [dbo].[oxite_Comment]'
GO
ALTER TABLE [dbo].[oxite_Comment] ADD CONSTRAINT [PK_oxite_Comment] PRIMARY KEY CLUSTERED ([CommentID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_CommentAnonymous]'
GO
CREATE TABLE [dbo].[oxite_CommentAnonymous]
(
[CommentID] [uniqueidentifier] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HashedEmail] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Url] [nvarchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_CommentAnonymous] on [dbo].[oxite_CommentAnonymous]'
GO
ALTER TABLE [dbo].[oxite_CommentAnonymous] ADD CONSTRAINT [PK_oxite_CommentAnonymous] PRIMARY KEY CLUSTERED ([CommentID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Plugin]'
GO
CREATE TABLE [dbo].[oxite_Plugin]
(
[SiteID] [uniqueidentifier] NOT NULL,
[PluginID] [uniqueidentifier] NOT NULL,
[PluginName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PluginCategory] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Enabled] [bit] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Plugin] on [dbo].[oxite_Plugin]'
GO
ALTER TABLE [dbo].[oxite_Plugin] ADD CONSTRAINT [PK_oxite_Plugin] PRIMARY KEY CLUSTERED ([SiteID], [PluginID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_PluginSetting]'
GO
CREATE TABLE [dbo].[oxite_PluginSetting]
(
[SiteID] [uniqueidentifier] NOT NULL,
[PluginID] [uniqueidentifier] NOT NULL,
[PluginSettingName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PluginSettingValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_PluginSetting] on [dbo].[oxite_PluginSetting]'
GO
ALTER TABLE [dbo].[oxite_PluginSetting] ADD CONSTRAINT [PK_oxite_PluginSetting] PRIMARY KEY CLUSTERED ([SiteID], [PluginID], [PluginSettingName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_PostAreaRelationship]'
GO
CREATE TABLE [dbo].[oxite_PostAreaRelationship]
(
[PostID] [uniqueidentifier] NOT NULL,
[AreaID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_PostAreaRelationship] on [dbo].[oxite_PostAreaRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostAreaRelationship] ADD CONSTRAINT [PK_oxite_PostAreaRelationship] PRIMARY KEY CLUSTERED ([PostID], [AreaID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_PostRelationship]'
GO
CREATE TABLE [dbo].[oxite_PostRelationship]
(
[SiteID] [uniqueidentifier] NOT NULL,
[ParentPostID] [uniqueidentifier] NOT NULL,
[PostID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_PostRelationship] on [dbo].[oxite_PostRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostRelationship] ADD CONSTRAINT [PK_oxite_PostRelationship] PRIMARY KEY CLUSTERED ([SiteID], [ParentPostID], [PostID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_PostTagRelationship]'
GO
CREATE TABLE [dbo].[oxite_PostTagRelationship]
(
[PostID] [uniqueidentifier] NOT NULL,
[TagID] [uniqueidentifier] NOT NULL,
[TagDisplayName] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_PostTagRelationship] on [dbo].[oxite_PostTagRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostTagRelationship] ADD CONSTRAINT [PK_oxite_PostTagRelationship] PRIMARY KEY CLUSTERED ([PostID], [TagID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_SiteRedirect]'
GO
CREATE TABLE [dbo].[oxite_SiteRedirect]
(
[SiteID] [uniqueidentifier] NOT NULL,
[SiteRedirect] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_SiteRedirect] on [dbo].[oxite_SiteRedirect]'
GO
ALTER TABLE [dbo].[oxite_SiteRedirect] ADD CONSTRAINT [PK_oxite_SiteRedirect] PRIMARY KEY CLUSTERED ([SiteID], [SiteRedirect])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_StringResource]'
GO
CREATE TABLE [dbo].[oxite_StringResource]
(
[StringResourceKey] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Language] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Version] [smallint] NOT NULL,
[StringResourceValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatorUserID] [uniqueidentifier] NOT NULL,
[CreatedDate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_StringResource] on [dbo].[oxite_StringResource]'
GO
ALTER TABLE [dbo].[oxite_StringResource] ADD CONSTRAINT [PK_oxite_StringResource] PRIMARY KEY CLUSTERED ([StringResourceKey])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_StringResourceVersion]'
GO
CREATE TABLE [dbo].[oxite_StringResourceVersion]
(
[StringResourceKey] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Language] [varchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Version] [smallint] NOT NULL,
[StringResourceValue] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreatorUserID] [uniqueidentifier] NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[State] [tinyint] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_StringResourceVersion] on [dbo].[oxite_StringResourceVersion]'
GO
ALTER TABLE [dbo].[oxite_StringResourceVersion] ADD CONSTRAINT [PK_oxite_StringResourceVersion] PRIMARY KEY CLUSTERED ([StringResourceKey], [Language], [Version])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_Subscription]'
GO
CREATE TABLE [dbo].[oxite_Subscription]
(
[SubscriptionID] [uniqueidentifier] NOT NULL,
[PostID] [uniqueidentifier] NOT NULL,
[UserID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_Subscription] on [dbo].[oxite_Subscription]'
GO
ALTER TABLE [dbo].[oxite_Subscription] ADD CONSTRAINT [PK_oxite_Subscription] PRIMARY KEY CLUSTERED ([SubscriptionID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_SubscriptionAnonymous]'
GO
CREATE TABLE [dbo].[oxite_SubscriptionAnonymous]
(
[SubscriptionID] [uniqueidentifier] NOT NULL,
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_SubscriptionAnonymous] on [dbo].[oxite_SubscriptionAnonymous]'
GO
ALTER TABLE [dbo].[oxite_SubscriptionAnonymous] ADD CONSTRAINT [PK_oxite_SubscriptionAnonymous] PRIMARY KEY CLUSTERED ([SubscriptionID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_UserFileResourceRelationship]'
GO
CREATE TABLE [dbo].[oxite_UserFileResourceRelationship]
(
[UserID] [uniqueidentifier] NOT NULL,
[FileResourceID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_UserFileResourceRelationship] on [dbo].[oxite_UserFileResourceRelationship]'
GO
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship] ADD CONSTRAINT [PK_oxite_UserFileResourceRelationship] PRIMARY KEY CLUSTERED ([UserID], [FileResourceID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_UserLanguage]'
GO
CREATE TABLE [dbo].[oxite_UserLanguage]
(
[UserID] [uniqueidentifier] NOT NULL,
[LanguageID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_UserLanguage] on [dbo].[oxite_UserLanguage]'
GO
ALTER TABLE [dbo].[oxite_UserLanguage] ADD CONSTRAINT [PK_oxite_UserLanguage] PRIMARY KEY CLUSTERED ([UserID], [LanguageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_UserRoleRelationship]'
GO
CREATE TABLE [dbo].[oxite_UserRoleRelationship]
(
[UserID] [uniqueidentifier] NOT NULL,
[RoleID] [uniqueidentifier] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_UserRoleRelationship] on [dbo].[oxite_UserRoleRelationship]'
GO
ALTER TABLE [dbo].[oxite_UserRoleRelationship] ADD CONSTRAINT [PK_oxite_UserRoleRelationship] PRIMARY KEY CLUSTERED ([UserID], [RoleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_MessageOutbound]'
GO
CREATE TABLE [dbo].[oxite_MessageOutbound]
(
[MessageOutboundID] [uniqueidentifier] NOT NULL,
[MessageTo] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MessageSubject] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MessageBody] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsSending] [bit] NOT NULL,
[RemainingRetryCount] [tinyint] NOT NULL,
[SentDate] [smalldatetime] NULL,
[LastAttemptDate] [smalldatetime] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_MessageOutbound] on [dbo].[oxite_MessageOutbound]'
GO
ALTER TABLE [dbo].[oxite_MessageOutbound] ADD CONSTRAINT [PK_oxite_MessageOutbound] PRIMARY KEY CLUSTERED ([MessageOutboundID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[oxite_TrackbackOutbound]'
GO
CREATE TABLE [dbo].[oxite_TrackbackOutbound]
(
[TrackbackOutboundID] [uniqueidentifier] NOT NULL,
[TargetUrl] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostID] [uniqueidentifier] NOT NULL,
[PostTitle] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostAreaTitle] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostBody] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostUrl] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsSending] [bit] NOT NULL,
[RemainingRetryCount] [tinyint] NOT NULL,
[SentDate] [smalldatetime] NULL,
[LastAttemptDate] [smalldatetime] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_oxite_TrackbackOutbound] on [dbo].[oxite_TrackbackOutbound]'
GO
ALTER TABLE [dbo].[oxite_TrackbackOutbound] ADD CONSTRAINT [PK_oxite_TrackbackOutbound] PRIMARY KEY CLUSTERED ([TrackbackOutboundID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[oxite_Role]'
GO
ALTER TABLE [dbo].[oxite_Role] ADD CONSTRAINT [IX_oxite_RoleName] UNIQUE NONCLUSTERED ([RoleName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[oxite_User]'
GO
ALTER TABLE [dbo].[oxite_User] ADD CONSTRAINT [IX_oxite_Username] UNIQUE NONCLUSTERED ([Username])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Area]'
GO
ALTER TABLE [dbo].[oxite_Area] ADD
CONSTRAINT [FK_oxite_Area_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_AreaRoleRelationship]'
GO
ALTER TABLE [dbo].[oxite_AreaRoleRelationship] ADD
CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Area] FOREIGN KEY ([AreaID]) REFERENCES [dbo].[oxite_Area] ([AreaID]),
CONSTRAINT [FK_oxite_AreaRoleRelationship_oxite_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[oxite_Role] ([RoleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_PostAreaRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostAreaRelationship] ADD
CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Area] FOREIGN KEY ([AreaID]) REFERENCES [dbo].[oxite_Area] ([AreaID]),
CONSTRAINT [FK_oxite_PostAreaRelationship_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Comment]'
GO
ALTER TABLE [dbo].[oxite_Comment] ADD
CONSTRAINT [FK_oxite_Comment_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID]),
CONSTRAINT [FK_oxite_Comment_oxite_User] FOREIGN KEY ([CreatorUserID]) REFERENCES [dbo].[oxite_User] ([UserID]),
CONSTRAINT [FK_oxite_Comment_oxite_Language] FOREIGN KEY ([LanguageID]) REFERENCES [dbo].[oxite_Language] ([LanguageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_CommentAnonymous]'
GO
ALTER TABLE [dbo].[oxite_CommentAnonymous] ADD
CONSTRAINT [FK_oxite_CommentAnonymous_oxite_Comment] FOREIGN KEY ([CommentID]) REFERENCES [dbo].[oxite_Comment] ([CommentID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_UserFileResourceRelationship]'
GO
ALTER TABLE [dbo].[oxite_UserFileResourceRelationship] ADD
CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_FileResource] FOREIGN KEY ([FileResourceID]) REFERENCES [dbo].[oxite_FileResource] ([FileResourceID]),
CONSTRAINT [FK_oxite_UserFileResourceRelationship_oxite_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_FileResource]'
GO
ALTER TABLE [dbo].[oxite_FileResource] ADD
CONSTRAINT [FK_oxite_FileResource_oxite_User] FOREIGN KEY ([CreatorUserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_User]'
GO
ALTER TABLE [dbo].[oxite_User] ADD
CONSTRAINT [FK_oxite_User_oxite_Language] FOREIGN KEY ([DefaultLanguageID]) REFERENCES [dbo].[oxite_Language] ([LanguageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_UserLanguage]'
GO
ALTER TABLE [dbo].[oxite_UserLanguage] ADD
CONSTRAINT [FK_oxite_UserLanguage_oxite_Language] FOREIGN KEY ([LanguageID]) REFERENCES [dbo].[oxite_Language] ([LanguageID]),
CONSTRAINT [FK_oxite_UserLanguage_oxite_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Plugin]'
GO
ALTER TABLE [dbo].[oxite_Plugin] ADD
CONSTRAINT [FK_oxite_Plugin_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_PluginSetting]'
GO
ALTER TABLE [dbo].[oxite_PluginSetting] ADD
CONSTRAINT [FK_oxite_PluginSetting_oxite_Plugin] FOREIGN KEY ([SiteID], [PluginID]) REFERENCES [dbo].[oxite_Plugin] ([SiteID], [PluginID]),
CONSTRAINT [FK_oxite_PluginSetting_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_PostRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostRelationship] ADD
CONSTRAINT [FK_oxite_PostRelationship_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID]),
CONSTRAINT [FK_oxite_PostRelationship_oxite_Post1] FOREIGN KEY ([ParentPostID]) REFERENCES [dbo].[oxite_Post] ([PostID]),
CONSTRAINT [FK_oxite_PostRelationship_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_PostTagRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostTagRelationship] ADD
CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID]),
CONSTRAINT [FK_oxite_PostTagRelationship_oxite_Tag] FOREIGN KEY ([TagID]) REFERENCES [dbo].[oxite_Tag] ([TagID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Subscription]'
GO
ALTER TABLE [dbo].[oxite_Subscription] ADD
CONSTRAINT [FK_oxite_Subscription_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID]),
CONSTRAINT [FK_oxite_Subscription_oxite_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Trackback]'
GO
ALTER TABLE [dbo].[oxite_Trackback] ADD
CONSTRAINT [FK_oxite_Trackback_oxite_Post] FOREIGN KEY ([PostID]) REFERENCES [dbo].[oxite_Post] ([PostID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Post]'
GO
ALTER TABLE [dbo].[oxite_Post] ADD
CONSTRAINT [FK_oxite_Post_oxite_User] FOREIGN KEY ([CreatorUserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Role]'
GO
ALTER TABLE [dbo].[oxite_Role] ADD
CONSTRAINT [FK_oxite_Role_oxite_Role] FOREIGN KEY ([ParentRoleID]) REFERENCES [dbo].[oxite_Role] ([RoleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_UserRoleRelationship]'
GO
ALTER TABLE [dbo].[oxite_UserRoleRelationship] ADD
CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_Role] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[oxite_Role] ([RoleID]),
CONSTRAINT [FK_oxite_UserRoleRelationship_oxite_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[oxite_User] ([UserID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_SiteRedirect]'
GO
ALTER TABLE [dbo].[oxite_SiteRedirect] ADD
CONSTRAINT [FK_oxite_SiteRedirect_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_StringResourceVersion]'
GO
ALTER TABLE [dbo].[oxite_StringResourceVersion] ADD
CONSTRAINT [FK_oxite_StringResourceVersion_oxite_StringResource] FOREIGN KEY ([StringResourceKey]) REFERENCES [dbo].[oxite_StringResource] ([StringResourceKey])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_SubscriptionAnonymous]'
GO
ALTER TABLE [dbo].[oxite_SubscriptionAnonymous] ADD
CONSTRAINT [FK_oxite_SubscriptionAnonymous_oxite_Subscription] FOREIGN KEY ([SubscriptionID]) REFERENCES [dbo].[oxite_Subscription] ([SubscriptionID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_Tag]'
GO
ALTER TABLE [dbo].[oxite_Tag] ADD
CONSTRAINT [FK_oxite_Tag_oxite_Tag] FOREIGN KEY ([ParentTagID]) REFERENCES [dbo].[oxite_Tag] ([TagID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO

/*Site record creation, uncomment to create a single site record, or run the site without a site
and you'll be prompted to supply all the needed information */

/*
DECLARE @Site1ID uniqueidentifier

SET @Site1ID = '4F36436B-0782-4a94-BB4C-FD3916734C03'

IF NOT EXISTS(SELECT * FROM oxite_Site WHERE SiteHost = 'http://localhost:30913')
BEGIN
	INSERT INTO
		oxite_Site
	(
		SiteID,
		SiteHost,
		SiteName,
		SiteDisplayName,
		SiteDescription,
		LanguageDefault,
		TimeZoneOffset,
		PageTitleSeparator,
		FavIconUrl,
		ScriptsPath,
		CssPath,
		CommentStateDefault,
		IncludeOpenSearch,
		AuthorAutoSubscribe,
		PostEditTimeout,
		GravatarDefault,
		SkinDefault,
		ServiceRetryCountDefault,
		HasMultipleAreas,
		RouteUrlPrefix,
		CommentingDisabled
	)
	VALUES
	(
		@Site1ID,
		'http://localhost:30913',
		'Oxite',
		'Oxite Sample',
		'This is the Oxite Sample description',
		'en',
		-8,
		' - ',
		'~/Content/icons/flame.ico',
		'~/Skins/{0}/Scripts',
		'~/Skins/{0}/Styles',
		'PendingApproval',
		1,
		1,
		24,
		'http://mschnlnine.vo.llnwd.net/d1/oxite/gravatar.jpg',
		'Default',
		10,
		0,
		'',
		0
	)
END



*/
