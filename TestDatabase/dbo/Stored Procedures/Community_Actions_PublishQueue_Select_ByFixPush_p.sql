

  CREATE proc [dbo].[Community_Actions_PublishQueue_Select_ByFixPush_p]  
as  
  
declare @StatDayStr varchar(19);  
set @StatDayStr=convert(varchar(19),GETDATE(),121)  
  
begin try  
 insert T_Community_Actions_Publish_JobLock(JobType,JobTime,LogTime)values('FIXPUSH',@StatDayStr,getdate())  
end try  
begin catch  
 return;  
end catch  
  
begin try  
 delete dbo.T_Community_Actions_PublishQueue where [Status]<>0 and RetryTimes>=2 and datediff(minute,getdate(),DealTime)<-5   
end try  
begin catch  
 print 'del'  
end catch  
  
begin try  
 update dbo.T_Community_Actions_PublishQueue set [Status]=0,RetryTimes=RetryTimes+1 where [Status]<>0 and RetryTimes<2 and datediff(minute,getdate(),DealTime)<-3
end try  
begin catch  
 print 'fix'  
end catch  
   
 declare @t table(Aid bigint,[Type] int)  
 insert into @t(Aid,[Type]) select top 100 Aid,[Type] from dbo.T_Community_Actions_PublishQueue where [Status]=0  
 update dbo.T_Community_Actions_PublishQueue set [Status]=1,DealTime=GETDATE() where Aid in (select Aid from @t)  
   
 select Aid,[TYPE] from @t  

