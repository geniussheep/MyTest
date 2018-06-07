
CREATE proc [dbo].[Community_Actions_ClearCacheActionsCheckLock_p]
as

declare @StatDayStr varchar(10);
set @StatDayStr=convert(varchar(10),GETDATE(),121)
begin try
	insert T_Community_Actions_Publish_JobLock(JobType,JobTime,LogTime)values('CLEARCACHEACTIONS',@StatDayStr,getdate())
end try
begin catch
	select 0;
	return;
end catch

select 1;

