create proc Community_Fans_Insert_p
@UserId int,
@FansId int

as

if not exists(select 1 from dbo.T_Community_Fans where UserID=@UserId and FansId=@FansId)
	insert dbo.T_Community_Fans(UserId,FansId)values(@UserId,@FansId)
	
