using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI.Model;

namespace KolejarzUI.Model
{
    public class Segment
    {
        public Segment(int id, TrainSet Train, Section section, string ETA, string ETD)
        {
            this.id = id;
            this.train = Train;
            this.ETA = ETA;
            this.ETD = ETD;
            this.section = section;
        }

        public static string SQLCreate = @"CREATE TABLE segment (
	        segment_id INT NOT NULL AUTO_INCREMENT,
	        section INT,
            trainset INT,
	        eta VARCHAR(20),
	        etd VARCHAR(20),	
            PRIMARY KEY (segment_id),
            CONSTRAINT FOREIGN KEY (section) REFERENCES section(section_id) 
                ON DELETE CASCADE,
            CONSTRAINT FOREIGN KEY (trainset) REFERENCES trainset(id)
                ON DELETE CASCADE
        ) ENGINE=INNODB;";

        public static string SQLDrop = "DROP TABLE IF EXISTS segment";
            
        public int id { get; set; }
        public TrainSet train { get; set; }
        public Section section { get; set; }
        public string ETA { get; set; }

        public string ETD { get; set; }

        private string FromStationName
        {
            get { return section?.from?.name; }
        }

        private string ToStationName
        {
            get { return section?.to?.name; }
        }

        private string trainSetName
        {
            get { return train?.shortName; }
        }

        public string trainSetFromToName
        {
            get { return trainSetName + ": " + FromStationName + " - " + ToStationName; }
        }
    }
}
