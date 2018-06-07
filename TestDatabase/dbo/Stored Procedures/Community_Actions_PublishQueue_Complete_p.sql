

	CREATE proc [dbo].[Community_Actions_PublishQueue_Complete_p]
@Id bigint,
@Type int

as

delete dbo.T_Community_Actions_PublishQueue where Aid=@Id and [Type]=@Type

