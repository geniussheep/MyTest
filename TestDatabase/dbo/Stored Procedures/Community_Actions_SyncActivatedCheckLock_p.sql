
 CREATE proc [dbo].[Community_Actions_SyncActivatedCheckLock_p]
as

declare @StatDayStr varchar(10);
set @StatDayStr=convert(varchar(10),GETDATE(),121)
begin try
	insert T_Community_Actions_Publish_JobLock(JobType,JobTime,LogTime)values('SYNCACTIVATEDUSER',@StatDayStr,getdate())
end try
begin catch
	select 0;
	return;
end catch

begin try
	delete T_Community_Actions_Publish_JobLock where LogTime<DATEADD(day,-3,getdate())
end try
begin catch
	print 'del'
end catch

	select 1;
