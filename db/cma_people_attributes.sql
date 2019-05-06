IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cma_people_attributes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cma_people_attributes]
GO

CREATE PROCEDURE [dbo].[cma_people_attributes]
	@userIds nvarchar(MAX)
AS
BEGIN

SELECT  
   AV.[object_id] AS ObjectId
  ,CA.[attribute_name] AS AttributeName
  ,AV.[attr_value] AS AttributeValue
  ,CA.[attribute_id] as AttributeId
  ,CA.[category] AS Category
  FROM         click_attribute_values AS AV WITH (NOLOCK)
INNER JOIN    click_attributes       AS CA WITH (NOLOCK) ON CA.[attribute_id] = AV.[attribute_id] 
                                                         AND CA.[category] IN ('Social Media','User Profile')                  
WHERE   CA.enable_p = 1
  AND   AV.object_id IN (SELECT value FROM string_split(@userIds, ','))
  AND   CA.parent_id = 0

END