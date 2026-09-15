using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI.Model;
using Kolejarz;

namespace KolejarzUI.Lists
{
    public class SegmentList : SQLFunctions
    {
        public List<Segment> segments { get; set; }

        private List<Section> sections { get; set; }

        public SegmentList(List<Segment> segments, List<Section> sections)
        {
            this.segments = segments;
            this.sections = sections;
        }

        public override void CreateTable()
        {
            cmd.Connection = mySQLConnection;
            cmd.CommandText = Segment.SQLCreate;
            Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        public override void DropTable()
        {
            cmd.Connection = mySQLConnection;
            cmd.CommandText = Segment.SQLDrop;
            Console.WriteLine(cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        public void AddSegment(Section section, TrainSet trainset, string ETA, string ETD)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Add_Segment({section.id}, {trainset.id}, \"{ETA}\", \"{ETD}\")";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportSegmentList();
        }

        public void DeleteSegment(Segment segment)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Delete_Segment({segment.id})";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.ExportSegmentList();
        }

        public override void FillTable()
        {
            foreach (Segment segment in segments)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = $"CALL Add_Segment({segment.section.id}, {segment.train.id}, \"{segment.ETA}\", \"{segment.ETD}\")";
                cmd.ExecuteNonQuery();
            }
        }

        public void ExportSegmentList()
        {
            segments = new List<Segment>();
            cmd.Parameters.Clear();
            cmd.CommandText = "CALL Export_Segment_List()";
            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    segments.Add(new Segment(reader.GetInt16(0), new TrainSet(reader.GetInt16(1), reader.GetString(2)), new Section(reader.GetInt32(3), new Station(reader.GetInt32(4), reader.GetString(5), reader.GetString(6), Convert.ToString(reader.GetFloat(7)), Convert.ToString(reader.GetFloat(8))), new Station(reader.GetInt32(9), reader.GetString(10), reader.GetString(11), Convert.ToString(reader.GetFloat(12)), Convert.ToString(reader.GetFloat(13))), Convert.ToString(reader.GetFloat(14))), reader.GetString(15), reader.GetString(16)));
                }
            }
        }
    }
}
