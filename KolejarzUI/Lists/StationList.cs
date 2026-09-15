using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kolejarz;
using KolejarzUI.Lists;
using KolejarzUI.Model;
using static System.Collections.Specialized.BitVector32;

namespace KolejarzUI.Lists
{
    public class StationList : SQLFunctions
    {
        public List<Station> stations { get; set; }

        public StationList(List<Station> stations)
        {
            this.stations = stations;
        }

        public override void CreateTable()
        {
            cmd.Connection = mySQLConnection;
            cmd.CommandText = Station.SQLCreate;
            Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "ALTER TABLE station MODIFY COLUMN station_name TEXT CHARACTER SET utf8";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "ALTER TABLE station MODIFY COLUMN station_address TEXT CHARACTER SET utf8";
            cmd.ExecuteNonQuery();
        }

        public override void DropTable()
        {
            cmd.Connection = mySQLConnection;
            cmd = new OdbcCommand(Station.SQLDrop, mySQLConnection);
            cmd.ExecuteNonQuery();
        }

        public override void FillTable()
        {
            foreach (Station station in stations)
            {
                cmd.Parameters.Clear();
                cmd.Connection = mySQLConnection;
                cmd.CommandText = $"CALL Add_Station(\"{station.name}\", \"{station.address}\", {station.longitude}, {station.latitude})";
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStation(Station station, SectionList sectionList, SegmentList segmentList)
        {
            cmd.Parameters.Clear();
            cmd.Connection = mySQLConnection;
            cmd.CommandText = $"CALL Delete_Station({station.id})";
            cmd.ExecuteNonQuery();
            this.ExportStationList();
            sectionList.ExportSectionList();
            segmentList.ExportSegmentList();
        }

        public void AddStation(string name, string address, string gps_longitude, string gps_latitude, SectionList sectionList, SegmentList segmentList)
        {
            cmd.Parameters.Clear();
            cmd.Connection = mySQLConnection;
            cmd.CommandText = $"CALL Add_Station(\"{name}\", \"{address}\", {gps_longitude}, {gps_latitude})";
            cmd.ExecuteNonQuery();
            this.ExportStationList();
            sectionList.ExportSectionList();
            segmentList.ExportSegmentList();
        }

        public void UpdateStation(Station station, string name, SectionList sectionList, SegmentList segmentList)
        {
            cmd.Parameters.Clear();
            cmd.Connection = mySQLConnection;
            cmd.CommandText = $"CALL Update_Station({station.id}, \"{name}\")";
            cmd.ExecuteNonQuery();
            this.ExportStationList();
            sectionList.ExportSectionList();
            segmentList.ExportSegmentList();
        }

        private void ExportStationList()
        {
            stations = new List<Station>();
            cmd.Parameters.Clear();
            cmd.CommandText = "CALL Export_Station_List()";

            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    stations.Add(new Station(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                }
            }
        }
    }
}
