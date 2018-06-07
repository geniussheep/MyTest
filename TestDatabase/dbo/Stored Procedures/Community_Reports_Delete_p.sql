CREATE PROCEDURE [dbo].[Community_Reports_Delete_p]
	@AppId NVARCHAR(20), 
    @ModuleId NVARCHAR(20), 
    @ArticleId NVARCHAR(20), 
    @UserId INT,
	@ReportedUserId INT
AS
	DELETE FROM [dbo].[T_Community_Reports] WHERE AppId = @AppId AND ArticleId =@ArticleId AND ModuleId  = @ModuleId AND (UserId = @UserId OR ReportedUserId = @ReportedUserId)

