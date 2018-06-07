create proc Community_Follow_Insert_p
@UserId int,
@FollowId int

as

if not exists(select 1 from dbo.T_Community_Follows where UserID=@UserId and FollowId=@FollowId)
	insert dbo.T_Community_Follows(UserId,FollowId)values(@UserId,@FollowId)