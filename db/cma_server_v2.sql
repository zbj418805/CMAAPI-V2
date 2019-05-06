IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cma_server_v2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[cma_server_v2]
GO

CREATE PROCEDURE [dbo].[cma_server_v2]
	@district_server_id int
AS
BEGIN
SELECT distinct cma.district_server_id  AS DistrictServerId,
	srv.server_id as ServerId, 
	srv.server_name as Name ,
	srv.description as Description,
	cma.geo_location.Lat as Latitude,
	cma.geo_location.Long as Longitude,
	srv.creation_date as TimeCreate,
	srv.last_modified as TimeUpdated, 
	url.url Url,
	srv.default_timezone as TimeZoneId,
	srv.default_locale  as Locale
	FROM v_click_servers srv
		INNER JOIN click_server_urls url on srv.server_id=url.server_id and url.default_p=1
		INNER JOIN cma_entries cma on srv.server_id= cma.server_id
		LEFT JOIN cma_entries news on srv.server_id= news.server_id and news.content_type='news'
		LEFT JOIN cma_entries calendar on srv.server_id= calendar.server_id and calendar.content_type='calendar'
		LEFT JOIN cma_entries people on srv.server_id= people.server_id and people.content_type='people'
	WHERE srv.server_id=@district_server_id or srv.parent_server_id = @district_server_id
END