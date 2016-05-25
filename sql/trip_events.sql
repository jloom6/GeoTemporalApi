CREATE TABLE IF NOT EXISTS `trip_events` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

INSERT INTO `trip_events` (`id`, `name`) VALUES
	(1, 'begin'),
	(2, 'update'),
	(3, 'end');
/*!40000 ALTER TABLE `trip_events` ENABLE KEYS */;