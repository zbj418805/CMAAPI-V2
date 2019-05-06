IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cma_people_userinfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [[dbo].[cma_people_userinfo]
GO

CREATE PROCEDURE [dbo].[cma_people_userinfo]
	@userIds nvarchar(MAX)
AS
BEGIN

SELECT
usr.user_id as UserId ,   
per.first_names as FirstName,    
per.last_name as LastName, 
ext.job_title as JobTitle,    
ext.work_phone as PhoneNumber, 
ext.description as Description,    
ext.priv_profile_picture as ImageUrl , 
par.email as Email      
FROM       users                 AS usr WITH (NOLOCK)
INNER JOIN persons               AS per WITH (NOLOCK)  ON usr.user_id=per.person_id
INNER JOIN parties               AS par WITH (NOLOCK)  ON usr.user_id=par.party_id
LEFT  JOIN user_extended         AS ext WITH (NOLOCK)  ON usr.user_id=ext.user_id
WHERE      usr.user_id IN (SELECT value FROM string_split(@userIds, ','))

END