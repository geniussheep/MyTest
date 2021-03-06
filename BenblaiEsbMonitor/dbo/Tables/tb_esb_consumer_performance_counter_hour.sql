﻿CREATE TABLE [dbo].[tb_esb_consumer_performance_counter_hour] (
    [id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [topic]            VARCHAR (512) NOT NULL,
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
    [from_date]        DATE          NOT NULL,
    [from_hour]        INT           NOT NULL,
    [processed]        BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

