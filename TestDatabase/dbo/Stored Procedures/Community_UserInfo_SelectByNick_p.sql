

CREATE proc [dbo].[Community_UserInfo_SelectByNick_p]
@NickName nvarchar(50)
as

select UserID,NickName,AvatarUrl,Sex,Intro,[Signature],BirthDate,Province,City,LastUpdateTime,Interest,Recommend,LastIp from [T_Community_UserInfo] with(nolock) where NickName=@NickName

