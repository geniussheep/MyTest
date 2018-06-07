
CREATE proc [dbo].[Community_Actions_PublishQueue_Insert_p]  
@Id bigint,
@Type int,
@InitTimes int 
  
as  
  
insert dbo.T_Community_Actions_PublishQueue(Aid,Status,Type,ReTryTimes,LogTime)  
 values(@Id,0,@Type,@InitTimes,GETDATE())  
  
