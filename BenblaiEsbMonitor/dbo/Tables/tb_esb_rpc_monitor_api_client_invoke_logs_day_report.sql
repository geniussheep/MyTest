CREATE TABLE [dbo].[tb_esb_rpc_monitor_api_client_invoke_logs_day_report] (
    [id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [domain_id]          INT            NOT NULL,
    [domain_address]     VARCHAR (512)  NOT NULL,
    [client_app_id]      INT            NOT NULL,
    [client_app_name]    VARCHAR (50)   NOT NULL,
    [server_id]          INT            NOT NULL,
    [server_ip]          VARCHAR (50)   NOT NULL,
    [path]               VARCHAR (2048) NOT NULL,
    [path_MD5]           VARCHAR (32)   NOT NULL,
    [invoke_count]       BIGINT         NOT NULL,
    [invoke_error_count] BIGINT         NOT NULL,
    [cost_time]          BIGINT         NOT NULL,
    [create_date]        DATE           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [client_day_report_unique] UNIQUE NONCLUSTERED ([domain_id] ASC, [domain_address] ASC, [client_app_id] ASC, [client_app_name] ASC, [server_id] ASC, [server_ip] ASC, [path_MD5] ASC, [create_date] ASC)
);

