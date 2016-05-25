BEGIN
	SELECT
		COUNT(*) TotalTrips,
		COALESCE(SUM(CASE WHEN t.fare IS NULL THEN 0 ELSE t.fare END), 0) TotalFares
	FROM
		trips t
			INNER JOIN trip_messages tmb ON (t.id = tmb.trip_id AND tmb.event_id = 1)
			LEFT JOIN trip_messages tme ON (t.id = tme.trip_id AND tme.event_id = 3)
	WHERE
		ST_Within(tmb.lat_long, ST_Buffer(ST_GeomFromText(CONCAT('POLYGON((', MinLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MinLongitude, '))')), 0.0000001)) OR
		ST_Within(tme.lat_long, ST_Buffer(ST_GeomFromText(CONCAT('POLYGON((', MinLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MinLongitude, '))')), 0.0000001));
END