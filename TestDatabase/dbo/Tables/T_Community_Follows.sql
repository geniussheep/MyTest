CREATE TABLE [dbo].[T_Community_Follows] (
    [UserId]   INT NOT NULL,
    [FollowId] INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_Community_Follows] PRIMARY KEY CLUSTERED ([UserId] ASC, [FollowId] ASC)
);

