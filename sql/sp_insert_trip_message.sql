BEGIN
	START TRANSACTION;
	SET @P = POINT(Latitude, Longitude);
	IF (EventId = 1)
		THEN
			INSERT INTO
				trips
					(id, begin_epoch, last_lat_long)
			VALUES
				(TripId, Epoch, @P);
		ELSE
			IF (EventId = 2)
				THEN
					UPDATE
						trips t
					SET
						t.last_lat_long = @P
					WHERE
						t.id = TripId;
				ELSE
					UPDATE
						trips t
					SET
						t.end_epoch = Epoch,
						t.fare = Fare,
						t.last_lat_long = @P
					WHERE
						t.id = TripId;
			END IF;
	END IF;
	INSERT INTO
		trip_messages
			(event_id, trip_id, lat_long, epoch, fare)
	VALUES
		(EventId, TripId, @P, Epoch, Fare);
	COMMIT;
END