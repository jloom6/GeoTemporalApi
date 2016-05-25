BEGIN
	SELECT
		tm.event_id `Event`,
		tm.trip_id TripId,
		ST_X(tm.lat_long) Latitude,
		ST_Y(tm.lat_long) Longitude,
		tm.fare Fare,
		tm.epoch Epoch
	FROM
		trip_messages tm
	WHERE
		tm.trip_id = TripId
	ORDER BY
		tm.epoch;
END