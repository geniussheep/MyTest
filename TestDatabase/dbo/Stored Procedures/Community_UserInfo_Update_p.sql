
CREATE proc [dbo].[Community_UserInfo_Update_p]
@UserId int,
@NickName nvarchar(50),
@AvatarUrl varchar(128),
@Sex bit,
@Intro nvarchar(256),
@Signature nvarchar(256),
@BirthDate Datetime,
@Province int,
@City int,
@Interest nvarchar(60),
@Recommend int,
@GuideState smallint,
@LastIp nvarchar(20)
as
if not exists(select 1 from [T_Community_UserInfo] where UserID=@UserId)
begin
	insert [T_Community_UserInfo](UserID,NickName,AvatarUrl,Sex,Intro,[Signature],BirthDate,Province,City,LastUpdateTime,Interest,Recommend,GuideState,LastIp)
		values(@UserID,@NickName,@AvatarUrl,@Sex,@Intro,@Signature,@BirthDate,@Province,@City,GETDATE(),@Interest,@Recommend,@GuideState,@LastIp)
end
else
begin
	update [T_Community_UserInfo]
		set 
			NickName=@NickName,
			AvatarUrl=@AvatarUrl,
			Sex=@Sex,
			Intro=@Intro,
			[Signature]=@Signature,
			BirthDate=@BirthDate,
			Province=@Province,
			City=@City,
			LastUpdateTime=GETDATE(),
			Interest = @Interest,
			Recommend= @Recommend,
			GuideState=@GuideState,
			LastIp =@LastIp
		where UserID=@UserId
end
