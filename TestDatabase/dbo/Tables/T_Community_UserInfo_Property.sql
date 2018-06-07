CREATE TABLE [dbo].[T_Community_UserInfo_Property] (
    [UserId]      INT NOT NULL,
    [FansCount]   INT DEFAULT ((0)) NOT NULL,
    [FollowCount] INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_T_Community_UserInfo_Property] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

