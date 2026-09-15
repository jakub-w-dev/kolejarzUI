using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI;

namespace KolejarzUI.Model
{
    public class Station 
    {
        public Station(int id, string name, string address, string longitude, string latitude)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public static string SQLCreate = "create table Station (station_id int not null auto_increment, " +
                "station_name varchar(60) not null, " +
                "station_address varchar(100) not null, " +
                "gps_latitude numeric(8,6) not null, " +
                "gps_longitude numeric(8,6) not null, " +
                "primary key (station_id)) ENGINE=INNODB;";

        public static string SQLDrop = "drop table if exists Station";
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }

        public void Display(string s) { Console.WriteLine(s + " | " + id + " | " + name + " | " + address + " | " + longitude + " | " + latitude + " |"); }

    }

}
