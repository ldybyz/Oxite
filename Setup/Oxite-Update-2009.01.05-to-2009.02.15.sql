/*
Script created by SQL Compare version 7.1.0 from Red Gate Software Ltd at 2/13/2009 11:07:13 AM
Run this script on an Oxite database from the January 5th, 2009 release (or before) to make it the 
same as the Oxite schema for the February 13th, 2009 release
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
PRINT N'Dropping foreign keys from [dbo].[oxite_CommentMessageRelationship]'
GO
ALTER TABLE [dbo].[oxite_CommentMessageRelationship] DROP
CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Comment],
CONSTRAINT [FK_oxite_CommentMessageRelationship_oxite_Message]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[oxite_CommentRelationship]'
GO
ALTER TABLE [dbo].[oxite_CommentRelationship] DROP
CONSTRAINT [FK_oxite_CommentRelationship_oxite_Comment]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[oxite_MessageTo]'
GO
ALTER TABLE [dbo].[oxite_MessageTo] DROP
CONSTRAINT [FK_oxite_MessageTo_oxite_Message],
CONSTRAINT [FK_oxite_MessageTo_oxite_User]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[oxite_Message]'
GO
ALTER TABLE [dbo].[oxite_Message] DROP
CONSTRAINT [FK_oxite_Message_oxite_User]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[oxite_MessageToAnonymous]'
GO
ALTER TABLE [dbo].[oxite_MessageToAnonymous] DROP
CONSTRAINT [FK_oxite_MessageToAnonymous_oxite_MessageTo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_BackgroundServiceAction]'
GO
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] DROP CONSTRAINT [PK_oxite_BackgroundServiceAction]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_BackgroundServiceAction]'
GO
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] DROP CONSTRAINT [DF_oxite_BackgroundServiceAction_BackgroundServiceActionID]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_BackgroundServiceAction]'
GO
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] DROP CONSTRAINT [DF_oxite_BackgroundServiceAction_InProgress]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_BackgroundServiceAction]'
GO
ALTER TABLE [dbo].[oxite_BackgroundServiceAction] DROP CONSTRAINT [DF_oxite_BackgroundServiceAction_Started]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_CommentMessageRelationship]'
GO
ALTER TABLE [dbo].[oxite_CommentMessageRelationship] DROP CONSTRAINT [PK_oxite_CommentMessageRelationship]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_CommentRelationship]'
GO
ALTER TABLE [dbo].[oxite_CommentRelationship] DROP CONSTRAINT [PK_oxite_CommentRelationship]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_Message]'
GO
ALTER TABLE [dbo].[oxite_Message] DROP CONSTRAINT [PK_oxite_Message]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_Message]'
GO
ALTER TABLE [dbo].[oxite_Message] DROP CONSTRAINT [DF_oxite_Message_MessageID]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_Message]'
GO
ALTER TABLE [dbo].[oxite_Message] DROP CONSTRAINT [DF_oxite_Message_IsSent]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_MessageTo]'
GO
ALTER TABLE [dbo].[oxite_MessageTo] DROP CONSTRAINT [PK_oxite_MessageTo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[oxite_MessageToAnonymous]'
GO
ALTER TABLE [dbo].[oxite_MessageToAnonymous] DROP CONSTRAINT [PK_oxite_MessageToAnonymous]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_MessageToAnonymous]'
GO
DROP TABLE [dbo].[oxite_MessageToAnonymous]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_MessageTo]'
GO
DROP TABLE [dbo].[oxite_MessageTo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_CommentRelationship]'
GO
DROP TABLE [dbo].[oxite_CommentRelationship]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_CommentMessageRelationship]'
GO
DROP TABLE [dbo].[oxite_CommentMessageRelationship]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_BackgroundServiceAction]'
GO
DROP TABLE [dbo].[oxite_BackgroundServiceAction]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[oxite_Message]'
GO
DROP TABLE [dbo].[oxite_Message]
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


DECLARE @Site1ID uniqueidentifier

--Sites
SELECT TOP 1 @Site1ID = SiteID From oxite_Area

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


/* end insert site record */

PRINT N'Altering [dbo].[oxite_Post]'
GO
ALTER TABLE [dbo].[oxite_Post] ADD
[CommentingDisabled] [bit] NOT NULL CONSTRAINT [DF_oxite_Post_AllowComments] DEFAULT ((0))
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
PRINT N'Altering [dbo].[oxite_Area]'
GO
ALTER TABLE [dbo].[oxite_Area] ADD
[CommentingDisabled] [bit] NOT NULL CONSTRAINT [DF_oxite_Area_AllowComments] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[oxite_Area] DROP
COLUMN [Type],
COLUMN [TypeUrl]
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
PRINT N'Altering [dbo].[oxite_Comment]'
GO
ALTER TABLE [dbo].[oxite_Comment] DROP
COLUMN [PublishedDate]
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
PRINT N'Altering [dbo].[oxite_PostTagRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostTagRelationship] ADD
[TagDisplayName] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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
PRINT N'Adding foreign keys to [dbo].[oxite_Area]'
GO
ALTER TABLE [dbo].[oxite_Area] ADD
CONSTRAINT [FK_oxite_Area_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[oxite_PostRelationship]'
GO
ALTER TABLE [dbo].[oxite_PostRelationship] ADD
CONSTRAINT [FK_oxite_PostRelationship_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
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
PRINT N'Adding foreign keys to [dbo].[oxite_SiteRedirect]'
GO
ALTER TABLE [dbo].[oxite_SiteRedirect] ADD
CONSTRAINT [FK_oxite_SiteRedirect_oxite_Site] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[oxite_Site] ([SiteID])
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

