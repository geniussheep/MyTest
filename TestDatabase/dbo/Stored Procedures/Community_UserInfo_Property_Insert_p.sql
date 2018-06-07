
create proc Community_UserInfo_Property_Insert_p
@UserId int,
@FansCount int,
@FollowCount int
as

if not exists(select 1 from [T_Community_UserInfo_Property] where UserId=@UserId)
begin
	insert [T_Community_UserInfo_Property](UserId,FansCount,FollowCount)values(@UserId,@FansCount,@FollowCount)
end

