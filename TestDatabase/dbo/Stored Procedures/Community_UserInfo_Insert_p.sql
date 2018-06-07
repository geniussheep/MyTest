
CREATE proc [dbo].[Community_UserInfo_Insert_p]
@UserId int,
@NickName nvarchar(50),
@AvatarUrl varchar(128),
@Sex bit,
@Intro nvarchar(256),
@Signature nvarchar(256),
@BirthDate datetime,
@Province int,
@City int,
@Interest nvarchar(60),
@Recommend int,
@LastIp nvarchar(20)
as

if not exists(select 1 from [T_Community_UserInfo] where UserID=@UserId)
begin
	insert [T_Community_UserInfo](UserID,NickName,AvatarUrl,Sex,Intro,Signature,BirthDate,Province,City,LastUpdateTime,Interest,Recommend,LastIp)
		values(@UserID,@NickName,@AvatarUrl,@Sex,@Intro,@Signature,@BirthDate,@Province,@City,GETDATE(),@Interest,@Recommend,@LastIp)
end
