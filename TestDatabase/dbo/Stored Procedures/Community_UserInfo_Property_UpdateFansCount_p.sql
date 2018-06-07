
CREATE proc [dbo].[Community_UserInfo_Property_UpdateFansCount_p]
@UserId int,
@FansCount int
as

if not exists(select 1 from [T_Community_UserInfo_Property] where UserId=@UserId)
begin
	insert [T_Community_UserInfo_Property](UserId,FansCount,FollowCount)values(@UserId,@FansCount,0)
end
else
begin
	update [T_Community_UserInfo_Property] set FansCount=FansCount+@FansCount where UserId=@UserId
end

