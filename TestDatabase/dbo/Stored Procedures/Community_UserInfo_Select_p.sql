CREATE proc [dbo].[Community_UserInfo_Select_p]
@UserId int
as

select UserID,NickName,AvatarUrl,Sex,Intro,[Signature],BirthDate,Province,City,LastUpdateTime,Interest,Recommend,LastIp from [T_Community_UserInfo] with(nolock) where UserID=@UserId


