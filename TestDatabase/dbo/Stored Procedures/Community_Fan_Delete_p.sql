
create proc Community_Fan_Delete_p
@UserId int,
@FansId int

as

delete dbo.T_Community_Fans where UserID=@UserId and FansId=@FansId
	
