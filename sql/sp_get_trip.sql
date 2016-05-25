BEGIN
	SELECT
		t.id Id,
		t.begin_epoch BeginEpoch,
		t.end_epoch EndEpoch,
		t.fare Fare,
		MAX(tm.epoch) LastMessageEpoch,
		ST_X(t.last_lat_long) LastLatitude,
		ST_Y(t.last_lat_long) LastLongitude
	FROM
		trips t
			INNER JOIN trip_messages tm ON t.id = tm.trip_id
	WHERE
		t.id = TripId
	GROUP BY
		t.id;
END