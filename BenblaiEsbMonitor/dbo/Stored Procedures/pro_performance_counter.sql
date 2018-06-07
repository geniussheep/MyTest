
CREATE PROCEDURE pro_performance_counter 
AS 
DECLARE @t_error INT = 0 ;
DECLARE @process_time DATETIME;
SET @process_time=DATEADD(day, -200,GETDATE());
BEGIN  
        BEGIN TRANSACTION

        BEGIN TRY
-- 插入两张日志表      
INSERT INTO tb_esb_consumer_performance_counter_log (topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_time,to_time,created_time)
SELECT topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_time,to_time,created_time
FROM tb_esb_consumer_performance_counter WHERE created_time<@process_time;
INSERT INTO tb_esb_producer_performance_counter_log (topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_time,to_time,created_time)
SELECT topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_time,to_time,created_time
FROM tb_esb_producer_performance_counter WHERE created_time<@process_time;

-- 插入两张备份表
INSERT INTO tb_esb_consumer_performance_counter_bak (topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_time,to_time,created_time)
SELECT topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_time,to_time,created_time
FROM tb_esb_consumer_performance_counter_log;
INSERT INTO tb_esb_producer_performance_counter_bak (topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_time,to_time,created_time)
SELECT topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_time,to_time,created_time
FROM tb_esb_producer_performance_counter_log;
INSERT INTO tb_esb_consumer_performance_counter_day
(topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_date)
SELECT topic,group_id,server_id,client_app_id,client_server_id,SUM(message_count),SUM(message_size),SUM(cached_count),SUM(committed_count),SUM(reverted_count),SUM(error_count),CONVERT(varchar(10), from_time, 23) FROM tb_esb_consumer_performance_counter_log 
 GROUP BY topic,group_id,server_id,client_app_id,client_server_id,CONVERT(varchar(10), from_time, 23);
 INSERT INTO tb_esb_producer_performance_counter_day (topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_date)
SELECT topic,server_id,client_app_id,client_server_id,SUM(message_count),SUM(message_size),MAX(message_max_size),SUM(saved_count),SUM(published_count),SUM(invalid_count),CONVERT(varchar(10), from_time, 23) FROM tb_esb_producer_performance_counter_log
GROUP BY topic,server_id,client_app_id,client_server_id,CONVERT(varchar(10), from_time, 23);
 INSERT INTO tb_esb_consumer_performance_counter_hour (topic,group_id,server_id,client_app_id,client_server_id,message_count,message_size,cached_count,committed_count,reverted_count,error_count,from_date,from_hour)
 SELECT topic,group_id,server_id,client_app_id,client_server_id,SUM(message_count),SUM(message_size),SUM(cached_count),SUM(committed_count),SUM(reverted_count),SUM(error_count),CONVERT(varchar(10), from_time, 23),DATEPART(HH,from_time) FROM tb_esb_consumer_performance_counter_log
  GROUP BY topic,group_id,server_id,client_app_id,client_server_id,CONVERT(varchar(10), from_time, 23),DATEPART(HH,from_time);
 
 INSERT INTO tb_esb_producer_performance_counter_hour (topic,server_id,client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count,from_date,from_hour)
SELECT topic,server_id,client_app_id,client_server_id,SUM(message_count),SUM(message_size),MAX(message_max_size),SUM(saved_count),SUM(published_count),SUM(invalid_count),CONVERT(varchar(10), from_time, 23),DATEPART(HH,from_time) FROM tb_esb_producer_performance_counter_log
 GROUP BY topic,server_id,client_app_id,client_server_id,CONVERT(varchar(10), from_time, 23),DATEPART(HH,from_time);
 DELETE FROM tb_esb_producer_performance_counter WHERE created_time<@process_time;
 DELETE FROM tb_esb_consumer_performance_counter WHERE created_time<@process_time;


 MERGE tb_esb_performance_counter_day AS Tday
 USING (SELECT * FROM (
SELECT topic,server_id,from_date,client_app_id,producer_client_app_id,SUM(consumer_message_count) AS consumer_message_count, SUM(consumer_message_size) AS consumer_message_size, SUM(cached_count) AS cached_count, SUM(committed_count) committed_count, SUM(reverted_count) reverted_count, SUM(error_count) error_count, SUM(producer_message_count) producer_message_count, SUM(producer_message_size) producer_message_size, MAX(producer_message_max_size) producer_message_max_size, SUM(saved_count) saved_count, SUM(published_count) published_count, SUM(invalid_count) invalid_count
FROM 
(
SELECT topic,server_id,from_date,client_app_id,message_count AS consumer_message_count,message_size AS consumer_message_size,cached_count,committed_count,reverted_count,error_count,
0 AS producer_client_app_id,0 AS producer_client_server_id,0 AS producer_message_count,0 AS producer_message_size,0 AS producer_message_max_size,0 AS saved_count,0 AS published_count,0 AS invalid_count
FROM tb_esb_consumer_performance_counter_day WHERE processed=0
 UNION ALL
SELECT topic,server_id,from_date,0 AS consumer_client_app_id,0 AS consumer_message_count,0 AS consumer_message_size,0 consumer_cached_count,0 consumer_committed_count,0 consumer_reverted_count,0 consumer_error_count,
client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count
FROM tb_esb_producer_performance_counter_day WHERE processed=0
) AS tt
GROUP BY topic,server_id,from_date,client_app_id,producer_client_app_id
) AS S) AS Sday
ON (Tday.topic = Sday.topic AND Tday.server_id = Sday.server_id AND Tday.consumer_client_app_id = Sday.client_app_id AND Tday.producer_client_app_id = Sday.producer_client_app_id AND Tday.from_date = Sday.from_date) 
WHEN NOT MATCHED  
    THEN INSERT(
topic,server_id,from_date,consumer_client_app_id,producer_client_app_id,consumer_message_count,consumer_message_size,cached_count,committed_count,reverted_count,error_count,
producer_message_count,producer_message_size,message_max_size,saved_count,published_count,invalid_count
)
 VALUES(Sday.topic ,
                    Sday.server_id ,
                    Sday.from_date ,
                    Sday.client_app_id ,
                    Sday.producer_client_app_id ,
                    Sday.consumer_message_count ,
                    Sday.consumer_message_size ,
                    Sday.cached_count ,
                    Sday.committed_count ,
                    Sday.reverted_count ,
                    Sday.error_count ,
                    Sday.producer_message_count ,
                    Sday.producer_message_size ,
                    Sday.producer_message_max_size ,
                    Sday.saved_count ,
                    Sday.published_count ,
                    Sday.invalid_count)
WHEN MATCHED 
    THEN UPDATE SET Tday.consumer_message_count=Tday.consumer_message_count+Sday.consumer_message_count,
Tday.consumer_message_size=Tday.consumer_message_size+Sday.consumer_message_size,
Tday.cached_count=Tday.cached_count+Sday.cached_count,
Tday.committed_count=Tday.committed_count+Sday.committed_count,
Tday.reverted_count=Tday.reverted_count+Sday.reverted_count,
Tday.error_count=Tday.error_count+Sday.error_count,
Tday.producer_message_count=Tday.producer_message_count+Sday.producer_message_count,
Tday.producer_message_size=Tday.producer_message_size+Sday.producer_message_size,
Tday.message_max_size=MAX(Tday.message_max_size,Sday.producer_message_max_size),
Tday.saved_count=Tday.saved_count+Sday.saved_count,
Tday.published_count=Tday.published_count+Sday.published_count,
Tday.invalid_count=Tday.invalid_count+Sday.invalid_count;

-- union 两个时间表
 MERGE tb_esb_performance_counter_hour AS Thour
 USING (SELECT * FROM (
SELECT topic,server_id,from_date,from_hour,client_app_id,producer_client_app_id, SUM(consumer_message_count) AS consumer_message_count, SUM(consumer_message_size) AS consumer_message_size, SUM(cached_count) AS cached_count, SUM(committed_count) committed_count, SUM(reverted_count) reverted_count, SUM(error_count) error_count, SUM(producer_message_count) producer_message_count, SUM(producer_message_size) producer_message_size, MAX(producer_message_max_size) producer_message_max_size, SUM(saved_count) saved_count, SUM(published_count) published_count, SUM(invalid_count) invalid_count
FROM 
(
SELECT topic,server_id,from_date,from_hour,client_app_id,message_count AS consumer_message_count,message_size AS consumer_message_size,cached_count,committed_count,reverted_count,error_count,
0 AS producer_client_app_id,0 AS producer_client_server_id,0 AS producer_message_count,0 AS producer_message_size,0 AS producer_message_max_size,0 AS saved_count,0 AS published_count,0 AS invalid_count
FROM tb_esb_consumer_performance_counter_hour WHERE processed=0
UNION ALL
SELECT topic,server_id,from_date,from_hour,0 AS consumer_client_app_id,0 AS consumer_message_count,0 AS consumer_message_size,0 consumer_cached_count,0 consumer_committed_count,0 consumer_reverted_count,0 consumer_error_count,
client_app_id,client_server_id,message_count,message_size,message_max_size,saved_count,published_count,invalid_count
FROM tb_esb_producer_performance_counter_hour WHERE processed=0
) AS tt
GROUP BY topic,server_id,from_date,from_hour,client_app_id,producer_client_app_id
) as Source) as Shour
ON (Thour.topic = Shour.topic AND Thour.server_id = Shour.server_id AND Thour.consumer_client_app_id = Shour.client_app_id AND Thour.producer_client_app_id = Shour.producer_client_app_id AND Thour.from_date = Shour.from_date AND Thour.from_hour = Shour.from_hour) 
WHEN NOT MATCHED  
    THEN INSERT(
topic,server_id,from_date,from_hour,consumer_client_app_id,producer_client_app_id,
consumer_message_count,consumer_message_size,cached_count,committed_count,reverted_count,error_count,producer_message_count,producer_message_size,message_max_size,saved_count,published_count,invalid_count
)
VALUES(
Shour.topic,Shour.server_id,Shour.from_date,Shour.from_hour,Shour.client_app_id,Shour.producer_client_app_id,Shour.consumer_message_count,Shour.consumer_message_size,Shour.cached_count,Shour.committed_count,Shour.reverted_count,Shour.error_count,Shour.producer_message_count,Shour.producer_message_size,Shour.producer_message_max_size,Shour.saved_count,Shour.published_count,Shour.invalid_count
)
	WHEN MATCHED 
    THEN UPDATE SET Thour.consumer_message_count=Thour.consumer_message_count+Shour.consumer_message_count,
Thour.consumer_message_size=Thour.consumer_message_size+Shour.consumer_message_size,
Thour.cached_count=Thour.cached_count+Shour.cached_count,
Thour.committed_count=Thour.committed_count+Shour.committed_count,
Thour.reverted_count=Thour.reverted_count+Shour.reverted_count,
Thour.error_count=Thour.error_count+Shour.error_count,
Thour.producer_message_count=Thour.producer_message_count+Shour.producer_message_count,
Thour.producer_message_size=Thour.producer_message_size+Shour.producer_message_size,
Thour.message_max_size=MAX(Thour.message_max_size,Shour.producer_message_max_size),
Thour.saved_count=Thour.saved_count+Shour.saved_count,
Thour.published_count=Thour.published_count+Shour.published_count,
Thour.invalid_count=Thour.invalid_count+Shour.invalid_count;


UPDATE tb_esb_consumer_performance_counter_day SET processed=1 WHERE processed=0;
UPDATE tb_esb_producer_performance_counter_day SET processed=1 WHERE processed=0;
UPDATE tb_esb_consumer_performance_counter_hour SET processed=1 WHERE processed=0;
UPDATE tb_esb_producer_performance_counter_hour SET processed=1 WHERE processed=0;

END TRY
        BEGIN CATCH  
            SET @t_error = @t_error + 1	
        END CATCH
        IF @t_error <> 0 
            BEGIN 
                ROLLBACK TRANSACTION   
            END
        ELSE 
            BEGIN 
                COMMIT TRANSACTION
            END 
END
TRUNCATE TABLE tb_esb_consumer_performance_counter_log;
TRUNCATE TABLE tb_esb_producer_performance_counter_log;
--topic,server_id,consumer_client_app_id,producer_client_app_id,from_date
GO
