BEGIN
	SELECT
		COUNT(DISTINCT tm.trip_id)
	FROM
		trip_messages tm
	WHERE
		ST_Within(tm.lat_long, ST_Buffer(ST_GeomFromText(CONCAT('POLYGON((', MinLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MinLongitude, ', ', MaxLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MaxLongitude, ', ', MinLatitude, ' ', MinLongitude, '))')), 0.0000001));
END