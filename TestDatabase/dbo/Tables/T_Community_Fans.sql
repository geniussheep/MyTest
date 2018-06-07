CREATE TABLE [dbo].[T_Community_Fans] (
    [UserId] INT NOT NULL,
    [FansId] INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_Community_Fans] PRIMARY KEY CLUSTERED ([UserId] ASC, [FansId] ASC)
);

