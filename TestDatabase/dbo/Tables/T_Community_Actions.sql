CREATE TABLE [dbo].[T_Community_Actions] (
    [AId]    BIGINT         NOT NULL,
    [AppId]  INT            NOT NULL,
    [Action] INT            NOT NULL,
    [UserId] INT            NOT NULL,
    [Ticks]  BIGINT         NOT NULL,
    [Title]  NVARCHAR (64)  DEFAULT ('') NOT NULL,
    [Remark] NVARCHAR (256) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_T_Community_Actions] PRIMARY KEY CLUSTERED ([AId] ASC)
);

