create proc [dbo].[Community_Actions_ClearNoneActivatedCheckLock_p]
as

declare @StatDayStr varchar(16);
set @StatDayStr=convert(varchar(16),GETDATE(),121)
begin try
	insert T_Community_Actions_Publish_JobLock(JobType,JobTime,LogTime)values('CLEARNONEACTIVATEDUSER',@StatDayStr,getdate())
end try
begin catch
	select 0;
	return;
end catch

select 1;

