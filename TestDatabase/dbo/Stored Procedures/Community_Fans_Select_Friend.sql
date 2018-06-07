CREATE proc [dbo].[Community_Fans_Select_Friend]
@UserId int
as
select distinct FansId from dbo.T_Community_Fans  where UserId in(select FollowId from  dbo.T_Community_Follows  where UserId= @UserId) and  FansId<>@UserId  and  FansId not in (select FollowId from  dbo.T_Community_Follows  where UserId= @UserId)


