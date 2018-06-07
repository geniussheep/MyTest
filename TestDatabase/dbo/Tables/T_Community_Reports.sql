CREATE TABLE [dbo].[T_Community_Reports] (
    [AppId]              NVARCHAR (20)  NOT NULL,
    [ModuleId]           NVARCHAR (20)  NOT NULL,
    [ArticleId]          NVARCHAR (20)  NOT NULL,
    [UserId]             INT            NOT NULL,
    [UserName]           NVARCHAR (50)  NULL,
    [ReportType]         NVARCHAR (50)  NULL,
    [ReportDesc]         NVARCHAR (500) NULL,
    [ReportedUserId]     INT            NOT NULL,
    [ReportedUserName]   NVARCHAR (50)  NULL,
    [ReportedArticleURL] NVARCHAR (200) NOT NULL,
    [ReportedContent]    NVARCHAR (500) NULL,
    [CreateDate]         DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([AppId] ASC, [ModuleId] ASC, [ArticleId] ASC, [UserId] ASC)
);

