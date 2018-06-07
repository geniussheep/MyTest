CREATE TABLE [dbo].[T_Community_Actions_Publish_JobLock] (
    [JobType] VARCHAR (32) NOT NULL,
    [JobTime] VARCHAR (32) NOT NULL,
    [LogTime] DATETIME     DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_T_Community_Actions_Publish_JobLock] PRIMARY KEY CLUSTERED ([JobType] ASC, [JobTime] ASC)
);

