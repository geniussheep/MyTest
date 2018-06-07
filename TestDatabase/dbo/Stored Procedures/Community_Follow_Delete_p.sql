create proc Community_Follow_Delete_p
@UserId int,
@FollowId int

as

delete dbo.T_Community_Follows where UserID=@UserId and FollowId=@FollowId
	
