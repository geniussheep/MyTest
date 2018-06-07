
create proc Community_Fans_Select_p
@UserId int

as

select FansId from dbo.T_Community_Fans with(nolock) where UserID=@UserId
	
