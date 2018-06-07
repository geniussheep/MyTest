CREATE TABLE [dbo].[tb_esb_producer_performance_counter_bak] (
    [id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [topic]            VARCHAR (512) NOT NULL,
    [server_id]        VARCHAR (512) NOT NULL,
    [client_app_id]    VARCHAR (512) NOT NULL,
    [client_server_id] VARCHAR (512) NOT NULL,
    [message_count]    BIGINT        DEFAULT ((0)) NOT NULL,
    [message_size]     BIGINT        DEFAULT ((0)) NOT NULL,
    [message_max_size] BIGINT        DEFAULT ((0)) NOT NULL,
    [saved_count]      BIGINT        DEFAULT ((0)) NOT NULL,
    [published_count]  BIGINT        DEFAULT ((0)) NOT NULL,
    [invalid_count]    BIGINT        DEFAULT ((0)) NOT NULL,
    [from_time]        DATETIME      NOT NULL,
    [to_time]          DATETIME      NOT NULL,
    [created_time]     DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

