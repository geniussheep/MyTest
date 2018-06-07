
CREATE proc [dbo].[Community_GetUserInfo_Paged_p]
@PageIndex int,
@PageSize int,
@UserId int,
@RecordCount int out 
as
begin
 select * from (select *,ROW_NUMBER() over(order by UserId) as Number from T_Community_UserInfo ut
	where UserId not in (select FollowId from T_Community_Follows where UserId=@UserId) and UserId<>@UserId
 ) t
 where Number between (@PageIndex-1)*@PageSize+1 and @PageIndex*@PageSize;
 
 select @RecordCount=COUNT(userid) from T_Community_UserInfo ut where
  UserId not in (select FollowId from T_Community_Follows where UserId=ut.UserId) and UserId<>@UserId;
end
