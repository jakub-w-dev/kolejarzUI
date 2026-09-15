using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolejarzUI.Model
{
    public class Connection
    {
        public Station firstStation { get; set; }

        public Station lastStation { get; set; }
        public TrainSet train { get; set; }

        //public string ETA { get; set; }

        public Connection(Station firstStation, Station lastStation, TrainSet train)
        {
            this.firstStation = firstStation;
            this.lastStation = lastStation;
            this.train = train;
        }

        public string dataString
        {
            get { return train.shortName + " - " + firstStation.name + " - " + lastStation.name; }
        }

        public string trainName
        {
            get { return train.shortName; }
        }
    }
}
