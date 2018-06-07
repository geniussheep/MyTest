create proc Community_Follow_Select_p
@UserId int

as

select FollowId from dbo.T_Community_Follows with(nolock) where UserID=@UserId
	
