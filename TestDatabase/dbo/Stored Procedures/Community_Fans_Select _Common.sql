
Create proc [dbo].[Community_Fans_Select _Common]
@UserId int,
@Tid int
as
SELECT *  from( SELECT [FollowId]
  FROM [SG2EventDB].[dbo].T_Community_Follows where [UserId] = @UserId   
   intersect  
   SELECT [FansId]
  FROM [dbo].[T_Community_Fans] where [UserId] = @Tid) as a