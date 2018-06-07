CREATE TABLE [dbo].[tb_esb_rpc_monitor_api_server_invoke_logs] (
    [id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [domain_Id]       BIGINT         DEFAULT (NULL) NULL,
    [domain_address]  VARCHAR (255)  DEFAULT (NULL) NULL,
    [client_app_id]   INT            DEFAULT (NULL) NULL,
    [client_app_name] VARCHAR (512)  DEFAULT (NULL) NULL,
    [server_id]       BIGINT         DEFAULT (NULL) NULL,
    [server_ip]       VARCHAR (45)   DEFAULT (NULL) NULL,
    [path]            VARCHAR (2048) DEFAULT (NULL) NULL,
    [error_count]     INT            DEFAULT (NULL) NULL,
    [invoke_count]    INT            DEFAULT (NULL) NULL,
    [min_time]        INT            DEFAULT (NULL) NULL,
    [max_time]        INT            DEFAULT (NULL) NULL,
    [avg_time]        INT            DEFAULT (NULL) NULL,
    [cost_time]       INT            DEFAULT (NULL) NULL,
    [created_time]    DATETIME       DEFAULT (NULL) NULL,
    [version]         VARCHAR (45)   DEFAULT (NULL) NULL,
    [protocol]        VARCHAR (45)   DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

