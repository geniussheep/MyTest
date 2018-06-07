CREATE TABLE [dbo].[T_Community_UserInfo] (
    [UserId]         INT            NOT NULL,
    [NickName]       NVARCHAR (50)  NOT NULL,
    [AvatarUrl]      VARCHAR (128)  DEFAULT ('') NOT NULL,
    [Sex]            BIT            DEFAULT ((0)) NOT NULL,
    [Intro]          NVARCHAR (256) DEFAULT ('') NOT NULL,
    [Signature]      NVARCHAR (256) DEFAULT ('') NOT NULL,
    [LastUpdateTime] DATETIME       DEFAULT (getdate()) NOT NULL,
    [Interest]       NVARCHAR (60)  CONSTRAINT [DF_T_Community_UserInfo_Interest] DEFAULT ('') NOT NULL,
    [Recommend]      INT            CONSTRAINT [DF_T_Community_UserInfo_Recommend] DEFAULT ((0)) NOT NULL,
    [GuideState]     SMALLINT       CONSTRAINT [DF_T_Community_UserInfo_GuideState] DEFAULT ((0)) NOT NULL,
    [BirthDate]      DATETIME       CONSTRAINT [DF_T_Community_UserInfo_BirthDate] DEFAULT (getdate()) NOT NULL,
    [Province]       INT            DEFAULT ((0)) NULL,
    [City]           INT            DEFAULT ((0)) NULL,
    [LastIp]         NVARCHAR (20)  CONSTRAINT [DF_T_Community_UserInfo_LastIp] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_T_Community_UserInfo] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_T_Community_UserInfo]
    ON [dbo].[T_Community_UserInfo]([NickName] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'关注英雄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Community_UserInfo', @level2type = N'COLUMN', @level2name = N'Interest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否被推荐', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Community_UserInfo', @level2type = N'COLUMN', @level2name = N'Recommend';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否引导 0 未引导 1引导到第一步 2引导到第二步 3引导完成', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Community_UserInfo', @level2type = N'COLUMN', @level2name = N'GuideState';

