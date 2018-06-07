CREATE PROCEDURE [dbo].[Community_Reports_Insert_p]
	@AppId NVARCHAR(20), 
    @ModuleId NVARCHAR(20), 
    @ArticleId NVARCHAR(20), 
    @UserId INT, 
    @UserName NVARCHAR(50),
	@ReportType NVARCHAR(50),
	@ReportDesc NVARCHAR(500),
	@ReportedUserId INT,
	@ReportedUserName NVARCHAR(50) , 
    @ReportedArticleURL NVARCHAR(200),
	@ReportedContent NVARCHAR(500)
AS
BEGIN 
	INSERT INTO [dbo].[T_Community_Reports]
           ([AppId]
           ,[ModuleId]
           ,[ArticleId]
           ,[UserId]
           ,[UserName]
           ,[ReportType]
		   ,[ReportDesc]
           ,[ReportedUserId]
           ,[ReportedUserName]
           ,[ReportedArticleURL]
           ,[ReportedContent]
           ,[CreateDate])
     VALUES
           (@AppId,
           @ModuleId,
           @ArticleId,
           @UserId,
           @UserName,
           @ReportType,
		   @ReportDesc,
           @ReportedUserId,
           @ReportedUserName,
           @ReportedArticleURL,
		   @ReportedContent,
           GETDATE())
END 



