CREATE TABLE [dbo].[tb_esb_performance_counter_hour] (
    [id]                     BIGINT        IDENTITY (1, 1) NOT NULL,
    [topic]                  VARCHAR (200) NOT NULL,
    [server_id]              INT           NOT NULL,
    [consumer_client_app_id] INT           NOT NULL,
    [consumer_message_count] BIGINT        DEFAULT ((0)) NOT NULL,
    [consumer_message_size]  BIGINT        DEFAULT ((0)) NOT NULL,
    [cached_count]           BIGINT        DEFAULT ((0)) NOT NULL,
    [committed_count]        BIGINT        DEFAULT ((0)) NOT NULL,
    [reverted_count]         BIGINT        DEFAULT ((0)) NOT NULL,
    [error_count]            BIGINT        DEFAULT ((0)) NOT NULL,
    [producer_client_app_id] INT           NOT NULL,
    [producer_message_count] BIGINT        DEFAULT ((0)) NOT NULL,
    [producer_message_size]  BIGINT        DEFAULT ((0)) NOT NULL,
    [message_max_size]       BIGINT        DEFAULT ((0)) NOT NULL,
    [saved_count]            BIGINT        DEFAULT ((0)) NOT NULL,
    [published_count]        BIGINT        DEFAULT ((0)) NOT NULL,
    [invalid_count]          BIGINT        DEFAULT ((0)) NOT NULL,
    [from_date]              DATE          NOT NULL,
    [from_hour]              INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [topic_SerId_ConClientAppId_ProdClientAppId_FromDate_FromHour] UNIQUE NONCLUSTERED ([topic] ASC, [server_id] ASC, [consumer_client_app_id] ASC, [producer_client_app_id] ASC, [from_date] ASC, [from_hour] ASC)
);

