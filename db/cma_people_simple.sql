IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cma_people_simple]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cma_people_simple]
GO


--staff_directory_get_basic_users_info_by_groups

CREATE PROCEDURE [dbo].[cma_people_simple]
	@group_ids NVARCHAR(max)
AS

SET NOCOUNT ON

SELECT  A.user_id as UserId
	   ,ISNULL(A.job_title,'') as JobTitle
       ,P.first_names as FirstName
	   ,P.last_name as LastName
  FROM        group_approved_member_map  AS R WITH (NOLOCK)    
  INNER JOIN  persons                    AS P WITH (NOLOCK)    ON P.person_id = R.member_id
  INNER JOIN  user_extended              AS A WITH (NOLOCK)    ON P.person_id = A.user_id

 WHERE R.group_id IN (SELECT value FROM string_split(@group_ids, ','))