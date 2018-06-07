CREATE TABLE [dbo].[tb_esb_rpc_monitor_api_invoke_logs_day_report] (
    [id]                        BIGINT        IDENTITY (1, 1) NOT NULL,
    [domain_id]                 INT           NOT NULL,
    [domain_address]            VARCHAR (512) NOT NULL,
    [client_invoke_count]       BIGINT        NOT NULL,
    [client_invoke_error_count] BIGINT        NOT NULL,
    [client_cost_time]          BIGINT        NOT NULL,
    [server_invoke_count]       BIGINT        NOT NULL,
    [server_invoke_error_count] BIGINT        NOT NULL,
    [server_max_time]           INT           NOT NULL,
    [server_min_time]           INT           NOT NULL,
    [server_cost_time]          INT           NOT NULL,
    [create_date]               DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

