IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cma_people_settings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cma_people_settings]
GO

CREATE PROCEDURE [dbo].[cma_people_settings]
(
	@server_id int
)
AS
BEGIN

	SELECT TOP 1
	CONVERT(XML, CONVERT(XML, pi.custom_prefs).value(N'(/properties/property[@name="SelectedGroups"]/text())[1]',N'nvarchar(max)')) as SelectGroupsXML,
	CONVERT(XML, CONVERT(XML, pi.custom_prefs).value(N'(/properties/property[@name="ExcludedUsers"]/text())[1]', N'nvarchar(max)')) as ExcludedUsersXML,
	CONVERT(XML, CONVERT(XML, pi.custom_prefs).value(N'(/properties/property[@name="Attributes"]/text())[1]', N'nvarchar(max)')) as AttributesXML
	FROM ptl_portlet_instances pi
	INNER JOIN cma_entries cma ON pi.portlet_instance_id = cma.object_id
	WHERE cma.server_id = @server_id AND cma.content_type = 'people'
	
END