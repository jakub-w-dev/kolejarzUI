using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kolejarz;
using KolejarzUI.Lists;
using KolejarzUI.Model;

namespace KolejarzUI
{
    public class TrainSetList : SQLFunctions
    {
        public List<TrainSet> trainSets { get; set; }
        public List<Carriage> carriages { get; set; }

        private Station firstStation;
        private Station lastStation;

        private string SQLQuery = "";

        public TrainSetList(List<Carriage> carriages, List<TrainSet> trainSets)
        {
            this.trainSets = trainSets;
            this.carriages = carriages;
        }

        public override void CreateTable()
        {
            SQLQuery = TrainSet.SQLCreate;
            cmd.CommandText = SQLQuery;
            cmd.ExecuteNonQuery();
        }

        public override void DropTable()
        {
            SQLQuery = TrainSet.SQLDrop;
            cmd.CommandText = SQLQuery;
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
        }

        public void DeleteCarriage(Carriage carriage, TrainSet trainset)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Delete_Carriage({carriage.id})";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportCarriageList(trainset);
        }

        public void DeleteTrainSet(TrainSet trainSet)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Delete_TrainSet({trainSet.id})";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportTrainSetList();
        }

        public override void FillTable()
        {
            foreach (TrainSet trainSet in trainSets)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = $"CALL Add_TrainSet(\"{trainSet.shortName}\")";
                cmd.ExecuteNonQuery();
            }
        }

        public void ExportTrainSetList()
        {
            this.trainSets = new List<TrainSet>();
            cmd.Parameters.Clear();
            cmd.CommandText = "CALL Export_TrainSet_List()";
            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    this.trainSets.Add(new TrainSet(reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }

        public void UpdateTrainSet(TrainSet trainSet, string name)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Update_TrainSet({trainSet.id}, \"{name}\")";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportTrainSetList();
        }
        public void ExportCarriageList(TrainSet trainSet)
        {
            this.carriages = new List<Carriage>();
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Export_Carriage_List({trainSet.id})";

            cmd.Connection = mySQLConnection;

            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    carriages.Add(new Carriage(reader.GetInt32(0), reader.GetBoolean(1), reader.GetString(2)));
                }
            }
        }

        public void AddCarriage(TrainSet trainSet, bool hasEngine, string type)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Add_Carriage({trainSet.id}, {hasEngine}, \"{type}\")";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportCarriageList(trainSet);
        }

        public void UpdateCarriage(Carriage carriage, string newType, TrainSet trainSet)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Update_Carriage({carriage.id}, \"{newType}\")";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportCarriageList(trainSet);
        }

        public void AddTrainSet(string name)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Add_TrainSet(\"{name}\")";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteScalar();
            this.ExportTrainSetList();
        }

        public Station ReturnFirstStation(TrainSet trainSet)
        {
            cmd.Parameters.Clear();
            SQLQuery = @"SELECT s.station_id, s.station_name, s.station_address, s.gps_latitude, s.gps_longitude
                                FROM trainset t
                                JOIN segment ON segment.trainset = t.id
                                JOIN section ON section.section_id = segment.section
                                JOIN station s ON s.station_id = section.departure
                                JOIN station ON station.station_id = section.destination
                                WHERE segment.segment_id = (
	                                SELECT MIN(segment_id) 
	                                FROM segment 
	                                WHERE trainset = " + trainSet.id + ");";
            cmd.CommandText = SQLQuery;
            cmd.Connection = mySQLConnection;

            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    firstStation = new Station(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                }
            }
            return firstStation;
        }

        public bool DoesTrainSetBelongToSegment(TrainSet trainSet)
        {
            cmd.Parameters.Clear();
            SQLQuery = @"SELECT COUNT(segment.segment_id) 
                        FROM segment
                        WHERE trainset = " + trainSet.id;
            cmd.CommandText = SQLQuery;
            cmd.Connection = mySQLConnection;
            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Station ReturnLastStation(TrainSet trainSet)
        {
            cmd.Parameters.Clear();
            SQLQuery = @"SELECT station.station_id, station.station_name, station.station_address, station.gps_latitude, station.gps_longitude
                                FROM trainset t
                                JOIN segment ON segment.trainset = t.id
                                JOIN section ON section.section_id = segment.section
                                JOIN station s ON s.station_id = section.departure
                                JOIN station ON station.station_id = section.destination
                                WHERE segment.segment_id = (
	                                SELECT MAX(segment_id) 
	                                FROM segment 
	                                WHERE trainset = " + trainSet.id + ");";
            cmd.CommandText = SQLQuery;
            cmd.Connection = mySQLConnection;

            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lastStation = new Station(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                }
            }
            return lastStation;
        }
    }
}
