CREATE TABLE IF NOT EXISTS `trip_messages` (
  `event_id` int(11) NOT NULL,
  `trip_id` int(11) NOT NULL,
  `lat_long` point NOT NULL,
  `epoch` bigint(20) NOT NULL,
  `fare` decimal(10,2) DEFAULT NULL,
  UNIQUE KEY `trip_id_epoch` (`trip_id`,`epoch`),
  KEY `trip_id` (`trip_id`),
  KEY `event` (`event_id`),
  SPATIAL KEY `lat_long` (`lat_long`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;