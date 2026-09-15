using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI.Model;
using static System.Collections.Specialized.BitVector32;

namespace KolejarzUI.Model
{
    public class Section
    {
        public Section(int id, Station from, Station to, string distance)
        {
            this.id = id;
            this.from = from;
            this.to = to;
            this.distance = distance;
        }
        
        public int id { get; set; }

        public static string SQLCreate = @"CREATE TABLE section (
            section_id INT NOT NULL AUTO_INCREMENT,
            departure INT, 
            destination INT, 
            distance FLOAT,
            PRIMARY KEY (section_id),
            CONSTRAINT FOREIGN KEY (departure) REFERENCES station(station_id) 
                ON DELETE CASCADE,
            CONSTRAINT FOREIGN KEY (destination) REFERENCES station(station_id) 
                ON DELETE CASCADE
        ) ENGINE=INNODB;";

        public static string SQLDrop = "drop table if exists section";
        public Station from { get; set; } 
        public Station to { get; set; }
        public string distance { get; set; }

        private string FromStationName
        {
            get { return from?.name; }
        }

        private string ToStationName
        {
            get { return to?.name; }
        }

        private string Distance 
        { 
            get { return distance; }
        }

        public string FromToName
        {
            get { return FromStationName + " - " + ToStationName + ": " + Distance + "km"; }
        }

    }
}
