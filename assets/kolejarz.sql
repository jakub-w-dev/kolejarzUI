-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 17, 2024 at 02:20 AM
-- Wersja serwera: 10.4.28-MariaDB
-- Wersja PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `kolejarz`
--

DELIMITER $$
--
-- Procedury
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `Add_Carriage` (IN `tid` INT, IN `has_engine` BOOLEAN, IN `carriage_type` VARCHAR(255))   BEGIN
                    INSERT INTO carriage(trainset_id, hasEngine, type) 
                    VALUES (tid, has_engine, carriage_type);
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Add_Section` (IN `departure_id` INT, IN `destination_id` INT, IN `distance_value` FLOAT)   BEGIN
    INSERT INTO section(departure, destination, distance) 
    VALUES (departure_id, destination_id, distance_value);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Add_Segment` (IN `section_id` INT, IN `trainset_id` INT, IN `eta_value` VARCHAR(20), IN `etd_value` VARCHAR(20))   BEGIN
    INSERT INTO segment(section, trainset, eta, etd) 
    VALUES (section_id, trainset_id, eta_value, etd_value);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Add_Station` (IN `name` VARCHAR(255), IN `address` VARCHAR(255), IN `longitude` DECIMAL(10,6), IN `latitude` DECIMAL(10,6))   BEGIN
                    INSERT INTO station(station_name, station_address, gps_longitude, gps_latitude) 
                    VALUES (name, address, longitude, latitude);
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Add_TrainSet` (IN `trainset_name` VARCHAR(255))   BEGIN
                    INSERT INTO trainset(name) VALUES (trainset_name);
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Delete_Carriage` (IN `carriage_id` INT)   BEGIN
                    DELETE FROM carriage WHERE id = carriage_id;
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Delete_Section` (IN `id` INT)   BEGIN
    DELETE FROM section WHERE section_id = id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Delete_Segment` (IN `id` INT)   BEGIN
    DELETE FROM segment WHERE segment_id = id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Delete_Station` (IN `id` INT)   BEGIN
DELETE FROM station WHERE station_id = id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Delete_TrainSet` (IN `trainset_id` INT)   BEGIN
                    DELETE FROM trainset WHERE id = trainset_id;
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Export_Carriage_List` (IN `cid` INT)   BEGIN
                    SELECT id, hasEngine, type 
                    FROM carriage 
                    WHERE trainset_id = cid;
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Export_Section_List` ()   BEGIN
SELECT section.section_id, section.distance, station.station_name, station.station_address, station.gps_latitude, station.gps_longitude, s.station_name, s.station_address, s.gps_latitude, s.gps_longitude, station.station_id, s.station_id
                                FROM section 
                                JOIN station ON station.station_id = section.departure
                                JOIN station s ON s.station_id = section.destination;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Export_Segment_List` ()   BEGIN
SELECT segment.segment_id, t.id, t.name, section.section_id, station.station_id, station.station_name, station.station_address, station.gps_latitude, station.gps_longitude, s.station_id, s.station_name, s.station_address, s.gps_latitude, s.gps_longitude, section.distance, segment.eta, segment.etd
                                FROM segment
                                JOIN trainset t ON t.id = segment.trainset
                                JOIN section ON section.section_id = segment.section
                                JOIN station ON station.station_id = section.departure
                                JOIN station s ON s.station_id = section.destination
                                ORDER BY t.id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Export_Station_List` ()   BEGIN
    SELECT station_id, station_name, station_address, gps_latitude, gps_longitude FROM station;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Export_TrainSet_List` ()   BEGIN
                    SELECT id, name FROM trainset;
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Update_Carriage` (IN `carriage_id` INT, IN `new_type` VARCHAR(255))   BEGIN
                    UPDATE carriage SET type = new_type WHERE id = carriage_id;
                END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Update_Station` (IN `id` INT, IN `name` VARCHAR(30))   BEGIN
UPDATE station SET station_name = name WHERE station_id = id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Update_TrainSet` (IN `trainset_id` INT, IN `new_name` VARCHAR(255))   BEGIN
                    UPDATE trainset SET name = new_name WHERE id = trainset_id;
                END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `carriage`
--

CREATE TABLE `carriage` (
  `id` int(11) NOT NULL,
  `trainset_id` int(11) DEFAULT NULL,
  `hasEngine` tinyint(1) DEFAULT NULL,
  `type` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `carriage`
--

INSERT INTO `carriage` (`id`, `trainset_id`, `hasEngine`, `type`) VALUES
(1, 9, 0, 'Food Carriage'),
(2, 1, 1, 'Conductor Carriage'),
(3, 9, 0, 'Food Carriage'),
(4, 6, 1, 'Conductor Carriage'),
(5, 1, 1, 'Conductor Carriage'),
(6, 2, 1, 'Conductor Carriage'),
(7, 5, 1, 'Conductor Carriage'),
(8, 3, 0, 'Passenger Carriage'),
(9, 7, 0, 'Passenger Carriage'),
(10, 7, 0, 'Food Carriage'),
(11, 6, 0, 'Food Carriage'),
(12, 1, 0, 'Food Carriage'),
(13, 8, 0, 'Passenger Carriage'),
(14, 4, 1, 'Conductor Carriage'),
(15, 5, 0, 'Passenger Carriage'),
(16, 3, 0, 'Passenger Carriage'),
(17, 10, 1, 'Conductor Carriage'),
(18, 4, 1, 'Conductor Carriage'),
(19, 3, 0, 'Food Carriage'),
(20, 4, 0, 'Food Carriage'),
(21, 9, 0, 'Passenger Carriage'),
(22, 6, 0, 'Food Carriage'),
(23, 7, 0, 'Food Carriage'),
(24, 1, 0, 'Food Carriage'),
(25, 9, 1, 'Conductor Carriage'),
(26, 2, 1, 'Conductor Carriage'),
(27, 10, 0, 'Food Carriage'),
(28, 1, 1, 'Conductor Carriage'),
(29, 3, 1, 'Conductor Carriage'),
(30, 3, 0, 'Passenger Carriage');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `section`
--

CREATE TABLE `section` (
  `section_id` int(11) NOT NULL,
  `departure` int(11) DEFAULT NULL,
  `destination` int(11) DEFAULT NULL,
  `distance` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `section`
--

INSERT INTO `section` (`section_id`, `departure`, `destination`, `distance`) VALUES
(1, 1, 2, 12),
(2, 2, 3, 47),
(3, 3, 4, 23),
(4, 4, 5, 46),
(5, 5, 6, 30),
(6, 6, 7, 49.59),
(7, 7, 8, 45),
(8, 8, 9, 24.21),
(9, 9, 10, 67),
(10, 10, 11, 3),
(11, 11, 12, 4);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `segment`
--

CREATE TABLE `segment` (
  `segment_id` int(11) NOT NULL,
  `section` int(11) DEFAULT NULL,
  `trainset` int(11) DEFAULT NULL,
  `eta` varchar(20) DEFAULT NULL,
  `etd` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `segment`
--

INSERT INTO `segment` (`segment_id`, `section`, `trainset`, `eta`, `etd`) VALUES
(1, 1, 1, '13:27', '13:45'),
(2, 2, 1, '13:47', '14:06'),
(3, 3, 1, '14:08', '14:21'),
(4, 4, 1, '14:22', '14:35'),
(5, 5, 1, '14:36', '14:52'),
(6, 6, 1, '14:53', '15:15'),
(7, 7, 1, '15:16', '15:35'),
(8, 8, 1, '15:36', '15:48'),
(9, 9, 1, '15:49', '16:18'),
(10, 10, 1, '16:20', '16:27'),
(11, 11, 1, '16:50', '16:56');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `station`
--

CREATE TABLE `station` (
  `station_id` int(11) NOT NULL,
  `station_name` text CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `station_address` text CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `gps_latitude` decimal(8,6) NOT NULL,
  `gps_longitude` decimal(8,6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `station`
--

INSERT INTO `station` (`station_id`, `station_name`, `station_address`, `gps_latitude`, `gps_longitude`) VALUES
(1, 'Poznań Główny', 'DWORCOWA 1; 61-801 POZNAŃ', 16.911893, 52.401444),
(2, 'Swarzędz', 'DWORCOWA 24; 62-020 SWARZĘDZ', 17.074498, 52.404153),
(3, 'Września', 'DWORCOWA 1; 62-300 WRZEŚNIA', 17.555379, 52.328203),
(4, 'Słupca', 'KOLEJOWA 1; 62-400 SŁUPCA', 17.852032, 52.287386),
(5, 'Konin', 'KOLEJOWA 1; 62-510 KONIN', 18.254054, 52.231084),
(6, 'Koło', 'KOLEJOWA 1; 62-600 KOŁO', 18.632003, 52.211028),
(7, 'Kutno', '3-GO MAJA; 99-300 KUTNO', 19.347845, 52.227192),
(8, 'Łowicz Główny', 'DWORCOWA 5; 99-400 ŁOWICZ', 19.956078, 52.104326),
(9, 'Sochaczew', 'SIENKIEWICZA 4; 96-500 SOCHACZEW', 20.239823, 52.215460),
(10, 'Warszawa Zachodnia', 'AL.JEROZOLIMSKIE 142; 00-811 WARSZAWA', 20.965180, 52.220090),
(11, 'Warszawa Centralna', 'AL.JEROZOLIMSKIE 54; 00-024 WARSZAWA', 21.002728, 52.228697),
(12, 'Warszawa Wschodnia', 'LUBELSKA 1; 03-802 WARSZAWA', 21.052459, 52.251274);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `trainset`
--

CREATE TABLE `trainset` (
  `id` int(11) NOT NULL,
  `name` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `trainset`
--

INSERT INTO `trainset` (`id`, `name`) VALUES
(1, 'IC 81103'),
(2, 'WO 3156'),
(3, 'KY 5453'),
(4, 'VD 0718'),
(5, 'HY 3460'),
(6, 'XQ 3486'),
(7, 'VB 4537'),
(8, 'QE 1562'),
(9, 'CD 2258'),
(10, 'KP 6124'),
(11, 'IU 1457'),
(12, 'NQ 3001');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `carriage`
--
ALTER TABLE `carriage`
  ADD PRIMARY KEY (`id`),
  ADD KEY `trainset_id` (`trainset_id`);

--
-- Indeksy dla tabeli `section`
--
ALTER TABLE `section`
  ADD PRIMARY KEY (`section_id`),
  ADD KEY `departure` (`departure`),
  ADD KEY `destination` (`destination`);

--
-- Indeksy dla tabeli `segment`
--
ALTER TABLE `segment`
  ADD PRIMARY KEY (`segment_id`),
  ADD KEY `section` (`section`),
  ADD KEY `trainset` (`trainset`);

--
-- Indeksy dla tabeli `station`
--
ALTER TABLE `station`
  ADD PRIMARY KEY (`station_id`);

--
-- Indeksy dla tabeli `trainset`
--
ALTER TABLE `trainset`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `carriage`
--
ALTER TABLE `carriage`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `section`
--
ALTER TABLE `section`
  MODIFY `section_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `segment`
--
ALTER TABLE `segment`
  MODIFY `segment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `station`
--
ALTER TABLE `station`
  MODIFY `station_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `trainset`
--
ALTER TABLE `trainset`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `carriage`
--
ALTER TABLE `carriage`
  ADD CONSTRAINT `carriage_ibfk_1` FOREIGN KEY (`trainset_id`) REFERENCES `trainset` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `section`
--
ALTER TABLE `section`
  ADD CONSTRAINT `section_ibfk_1` FOREIGN KEY (`departure`) REFERENCES `station` (`station_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `section_ibfk_2` FOREIGN KEY (`destination`) REFERENCES `station` (`station_id`) ON DELETE CASCADE;

--
-- Constraints for table `segment`
--
ALTER TABLE `segment`
  ADD CONSTRAINT `segment_ibfk_1` FOREIGN KEY (`section`) REFERENCES `section` (`section_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `segment_ibfk_2` FOREIGN KEY (`trainset`) REFERENCES `trainset` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
