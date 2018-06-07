
create proc [dbo].[Community_UserInfo_Property_Select_p]
@UserId int
as

select UserId,FansCount,FollowCount from [T_Community_UserInfo_Property] with(nolock) where UserID=@UserId


