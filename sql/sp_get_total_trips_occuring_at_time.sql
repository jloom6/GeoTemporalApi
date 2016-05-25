BEGIN
	SELECT
		COUNT(*)
	FROM
		trips t
	WHERE
		t.begin_epoch <= Epoch AND
	  (t.end_epoch IS NULL OR
		t.end_epoch >= Epoch);
END