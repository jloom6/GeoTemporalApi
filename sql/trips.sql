CREATE TABLE IF NOT EXISTS `trips` (
  `id` int(11) NOT NULL,
  `begin_epoch` bigint(20) NOT NULL,
  `end_epoch` bigint(20) DEFAULT NULL,
  `fare` decimal(10,2) DEFAULT NULL,
  `last_lat_long` point DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `begin_epoch` (`begin_epoch`),
  KEY `end_epoch` (`end_epoch`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;