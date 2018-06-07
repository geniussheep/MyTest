
Create proc [dbo].[Community_Follows_Select _Common]
@UserId int,
@Tid int
as
SELECT *  from( SELECT [FollowId]
  FROM [SG2EventDB].[dbo].T_Community_Follows where [UserId] = @UserId  
intersect
SELECT [FollowId] 
  FROM [SG2EventDB].[dbo].T_Community_Follows where [UserId] = @Tid ) as a