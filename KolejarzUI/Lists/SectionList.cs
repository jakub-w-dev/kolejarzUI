using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI;
using KolejarzUI.Forms;
using Kolejarz;
using KolejarzUI.Model;

namespace KolejarzUI.Lists
{
    public class SectionList : SQLFunctions
    {
        public List<Section> sections { get; set; }
        public SectionList(List<Section> Sections)
        {
            this.sections = Sections;
        }


        public override void CreateTable()
        {
            cmd.Connection = mySQLConnection;
            cmd.CommandText = Section.SQLCreate;
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
            cmd = new OdbcCommand(Section.SQLDrop, mySQLConnection);
            cmd.ExecuteNonQuery();
        }

        public void DeleteSection(Section section, SegmentList segmentList)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Delete_Section({section.id})";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.UpdateData(segmentList);
        }

        public void AddSection(Station stationFrom, Station stationTo, string distance, SegmentList segmentList)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"CALL Add_Section({stationFrom.id}, {stationTo.id}, {distance})";
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
            this.UpdateData(segmentList);
        }
        public override void FillTable()
        {
            foreach (Section section in sections)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = $"CALL Add_Section({section.from.id}, {section.to.id}, {section.distance})";
                cmd.ExecuteNonQuery();
            }
        }

        public void ExportSectionList()
        {
            this.sections = new List<Section>();
            cmd.Parameters.Clear();
            cmd.CommandText = "CALL Export_Section_List()";

            using (OdbcDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    sections.Add(new Section(reader.GetInt32(0), new Station(reader.GetInt32(10), reader.GetString(2), reader.GetString(3), Convert.ToString(reader.GetDecimal(4)), Convert.ToString(reader.GetDecimal(5))), new Station(reader.GetInt32(11), reader.GetString(6), reader.GetString(7), Convert.ToString(reader.GetDecimal(8)), Convert.ToString(reader.GetDecimal(9))), Convert.ToString(reader.GetFloat(1))));
                }
            }
        }

        private void UpdateData(SegmentList segmentList)
        {
            this.ExportSectionList();
            segmentList.ExportSegmentList();

            SectionsForm.sectionListBox.DataSource = null;
            SectionsForm.sectionListBox.DataSource = this.sections;
            SectionsForm.sectionListBox.DisplayMember = "FromToName";

            SegmentsForm.segmentListBox.DataSource = null;
            SegmentsForm.segmentListBox.DataSource = segmentList.segments;
            SegmentsForm.segmentListBox.DisplayMember = "trainSetFromToName";

            SegmentsForm.sectionListBox.DataSource = null;
            SegmentsForm.sectionListBox.DataSource = this.sections;
            SegmentsForm.sectionListBox.DisplayMember = "FromToName";
        }
    }
}
