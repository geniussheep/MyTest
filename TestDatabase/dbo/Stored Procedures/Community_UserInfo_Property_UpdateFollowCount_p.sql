
CREATE proc [dbo].[Community_UserInfo_Property_UpdateFollowCount_p]
@UserId int,
@FollowCount int
as

if not exists(select 1 from [T_Community_UserInfo_Property] where UserId=@UserId)
begin
	insert [T_Community_UserInfo_Property](UserId,FansCount,FollowCount)values(@UserId,0,@FollowCount)
end
else
begin
	update [T_Community_UserInfo_Property] set FollowCount=FollowCount+@FollowCount where UserId=@UserId
end

