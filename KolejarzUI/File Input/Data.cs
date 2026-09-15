using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI.Model;

namespace Kolejarz
{
    internal class Data
    {
        public List<Station> Stations { get; set; }
        public List<Section> Sections { get; set; }
        public List<Segment> Segments { get; set; }
        public Data(List<Station> stations, List<Section> sections, List<Segment> segments)
        {
            Stations = stations;
            Sections = sections;
            Segments = segments;
        }
    }
}
