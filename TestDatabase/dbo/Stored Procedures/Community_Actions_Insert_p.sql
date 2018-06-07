

CREATE proc [dbo].[Community_Actions_Insert_p]
@Id bigint,
@AppId int,
@Action int,
@UserId int,
@Ticks bigint,
@Title nvarchar(64),
@Remark nvarchar(256)

as

insert dbo.T_Community_Actions(Aid,AppId,Action,UserId,Ticks,Title,Remark)
	values(@Id,@AppId,@Action,@UserId,@Ticks,@Title,@Remark)

