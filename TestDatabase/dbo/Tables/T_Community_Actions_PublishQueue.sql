CREATE TABLE [dbo].[T_Community_Actions_PublishQueue] (
    [AId]        BIGINT   NOT NULL,
    [Status]     INT      NOT NULL,
    [Type]       INT      NOT NULL,
    [LogTime]    DATETIME DEFAULT (getdate()) NOT NULL,
    [ReTryTimes] INT      CONSTRAINT [DF_T_Community_Actions_PublishQueue_ReTryTimes] DEFAULT ((0)) NOT NULL,
    [DealTime]   DATETIME CONSTRAINT [DF_T_Community_Actions_PublishQueue_DealTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_T_Community_Actions_PublishQueue] PRIMARY KEY CLUSTERED ([AId] ASC, [Type] ASC)
);

