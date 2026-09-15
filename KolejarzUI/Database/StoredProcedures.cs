using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Kolejarz;

namespace KolejarzUI.Database
{
    public class StoredProcedures : DatabaseConnection
    {
        private List<string> queries = new List<string>();

        public void CreateStationStoredProcedures()
        {
            queries = new List<string>
            {
                @"DROP PROCEDURE IF EXISTS Add_Station;",
                @"
                CREATE PROCEDURE Add_Station(
                    IN p_name VARCHAR(255), 
                    IN p_address VARCHAR(255), 
                    IN p_longitude DECIMAL(10, 6), 
                    IN p_latitude DECIMAL(10, 6)
                )
                BEGIN
                    INSERT INTO station(station_name, station_address, gps_longitude, gps_latitude) 
                    VALUES (p_name, p_address, p_longitude, p_latitude);
                END;",
                @"DROP PROCEDURE IF EXISTS Delete_Station;",
                @"
                CREATE PROCEDURE Delete_Station(
                    IN p_station_id INT
                )
                BEGIN
                    DELETE FROM station WHERE station_id = p_station_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Update_Station;",
                @"
                CREATE PROCEDURE Update_Station(
                    IN p_station_id INT, 
                    IN p_name VARCHAR(255)
                )
                BEGIN
                    UPDATE station 
                    SET station_name = p_name 
                    WHERE station_id = p_station_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Export_Station_List;",
                @"
                CREATE PROCEDURE Export_Station_List()
                BEGIN
                    SELECT station_id, station_name, station_address, gps_latitude, gps_longitude FROM station;
                END;"
            };

            ExecuteQueries(queries);
        }

        public void CreateSectionStoredProcedures()
        {
            queries = new List<string>
            {
                @"DROP PROCEDURE IF EXISTS Delete_Section;",
                @"
                CREATE PROCEDURE Delete_Section(
                    IN id INT
                )
                BEGIN
                    DELETE FROM section WHERE section_id = id;
                END;",
                @"DROP PROCEDURE IF EXISTS Add_Section;",
                @"
                CREATE PROCEDURE Add_Section(
                    IN departure_id INT, 
                    IN destination_id INT, 
                    IN distance_value FLOAT
                )
                BEGIN
                    INSERT INTO section(departure, destination, distance) 
                    VALUES (departure_id, destination_id, distance_value);
                END;",
                @"DROP PROCEDURE IF EXISTS Export_Section_List;",
                @"
                CREATE PROCEDURE Export_Section_List()
                BEGIN
                    SELECT section.section_id, section.distance, station.station_name, station.station_address, station.gps_latitude, station.gps_longitude, s.station_name, s.station_address, s.gps_latitude, s.gps_longitude, station.station_id, s.station_id
                    FROM section 
                    JOIN station ON station.station_id = section.departure
                    JOIN station s ON s.station_id = section.destination;
                END;"
            };

            ExecuteQueries(queries);
        }

        public void CreateSegmentStoredProcedures()
        {
            queries = new List<string>
            {
                @"DROP PROCEDURE IF EXISTS Add_Segment;",
                @"
                CREATE PROCEDURE Add_Segment(
                    IN section_id INT, 
                    IN trainset_id INT, 
                    IN eta_value VARCHAR(20), 
                    IN etd_value VARCHAR(20)
                )
                BEGIN
                    INSERT INTO segment(section, trainset, eta, etd) 
                    VALUES (section_id, trainset_id, eta_value, etd_value);
                END;",
                @"DROP PROCEDURE IF EXISTS Delete_Segment;",
                @"
                CREATE PROCEDURE Delete_Segment(
                    IN id INT
                )
                BEGIN
                    DELETE FROM segment WHERE segment_id = id;
                END;",
                @"DROP PROCEDURE IF EXISTS Export_Segment_List;",
                @"
                CREATE PROCEDURE Export_Segment_List()
                BEGIN
                    SELECT segment.segment_id, t.id, t.name, section.section_id, station.station_id, station.station_name, station.station_address, station.gps_latitude, station.gps_longitude, s.station_id, s.station_name, s.station_address, s.gps_latitude, s.gps_longitude, section.distance, segment.eta, segment.etd
                    FROM segment
                    JOIN trainset t ON t.id = segment.trainset
                    JOIN section ON section.section_id = segment.section
                    JOIN station ON station.station_id = section.departure
                    JOIN station s ON s.station_id = section.destination
                    ORDER BY t.id;
                END;"
            };

            ExecuteQueries(queries);
        }

        public void CreateTrainSetStoredProcedures()
        {
            queries = new List<string>
            {
                @"DROP PROCEDURE IF EXISTS Add_TrainSet;",
                @"
                CREATE PROCEDURE Add_TrainSet(
                    IN trainset_name VARCHAR(255)
                )
                BEGIN
                    INSERT INTO trainset(name) VALUES (trainset_name);
                END;",
                @"DROP PROCEDURE IF EXISTS Delete_TrainSet;",
                @"
                CREATE PROCEDURE Delete_TrainSet(
                    IN trainset_id INT
                )
                BEGIN
                    DELETE FROM trainset WHERE id = trainset_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Update_TrainSet;",
                @"
                CREATE PROCEDURE Update_TrainSet(
                    IN trainset_id INT, 
                    IN new_name VARCHAR(255)
                )
                BEGIN
                    UPDATE trainset SET name = new_name WHERE id = trainset_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Export_TrainSet_List;",
                @"
                CREATE PROCEDURE Export_TrainSet_List()
                BEGIN
                    SELECT id, name FROM trainset;
                END;",
                @"DROP PROCEDURE IF EXISTS Add_Carriage;",
                @"
                CREATE PROCEDURE Add_Carriage(
                    IN tid INT, 
                    IN has_engine BOOLEAN, 
                    IN carriage_type VARCHAR(255)
                )
                BEGIN
                    INSERT INTO carriage(trainset_id, hasEngine, type) 
                    VALUES (tid, has_engine, carriage_type);
                END;",
                @"DROP PROCEDURE IF EXISTS Delete_Carriage;",
                @"
                CREATE PROCEDURE Delete_Carriage(
                    IN carriage_id INT
                )
                BEGIN
                    DELETE FROM carriage WHERE id = carriage_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Update_Carriage;",
                @"
                CREATE PROCEDURE Update_Carriage(
                    IN carriage_id INT, 
                    IN new_type VARCHAR(255)
                )
                BEGIN
                    UPDATE carriage SET type = new_type WHERE id = carriage_id;
                END;",
                @"DROP PROCEDURE IF EXISTS Export_Carriage_List;",
                @"
                CREATE PROCEDURE Export_Carriage_List(
                    IN cid INT
                )
                BEGIN
                    SELECT id, hasEngine, type 
                    FROM carriage 
                    WHERE trainset_id = cid;
                END;"
            };

            ExecuteQueries(queries);
        }


        private void ExecuteQueries(IEnumerable<string> queries)
        {
            foreach (var query in queries)
            {
                cmd.CommandText = query;
                cmd.Connection = mySQLConnection;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
