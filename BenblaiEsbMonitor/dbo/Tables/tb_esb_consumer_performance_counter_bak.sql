CREATE TABLE [dbo].[tb_esb_consumer_performance_counter_bak] (
    [id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [topic]            VARCHAR (200) NOT NULL,
    [group_id]         VARCHAR (512) NOT NULL,
    [server_id]        VARCHAR (512) NOT NULL,
    [client_app_id]    VARCHAR (512) NOT NULL,
    [client_server_id] VARCHAR (512) NOT NULL,
    [message_count]    BIGINT        DEFAULT ((0)) NOT NULL,
    [message_size]     BIGINT        DEFAULT ((0)) NOT NULL,
    [cached_count]     BIGINT        DEFAULT ((0)) NOT NULL,
    [committed_count]  BIGINT        DEFAULT ((0)) NOT NULL,
    [reverted_count]   BIGINT        DEFAULT ((0)) NOT NULL,
    [error_count]      BIGINT        DEFAULT ((0)) NOT NULL,
    [from_time]        DATETIME      NOT NULL,
    [to_time]          DATETIME      NOT NULL,
    [created_time]     DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

