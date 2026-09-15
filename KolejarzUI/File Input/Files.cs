using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI.Model;

namespace Kolejarz
{
    internal class Files
    {
        public Data ReadFromFiles(string path)
        {
            List<Station> stationList = ReadFromStations(path);
            List<Section> sectionList = ReadFromSections(path, stationList);
            List<Segment> segmentList = ReadFromSegments(path, sectionList);

            return new Data(stationList, sectionList, segmentList);
        }
        private List<Station> ReadFromStations(string path)
        {
            using (StreamReader reader = new StreamReader(path + "/Stations.csv"))
            {
                List<string> allLinesText = File.ReadAllLines(path + "/Stations.csv").ToList();

                List<Station> stationList = new List<Station>();

                for (int i = 0; i < allLinesText.Count; i++)
                {
                    string[] splitLine = Convert.ToString(allLinesText[i]).Split(',').Select(x => x.Trim()).ToArray();

                    stationList.Add(new Station(i+1, splitLine[0], splitLine[1], splitLine[2], splitLine[3]));
                }

                return stationList;
            }

        }
        private List<Section> ReadFromSections(string path, List<Station> stationList)
        {
            using (StreamReader reader = new StreamReader(path + "/Sections.csv"))
            {

                List<string> allLinesText = File.ReadAllLines(path + "/Sections.csv").ToList();

                List<Section> sectionList = new List<Section>();

                for (int i = 1; i <= allLinesText.Count; i++)
                {
                    string[] splitLine = Convert.ToString(allLinesText[i - 1]).Split(',').Select(x => x.Trim()).ToArray();

                    sectionList.Add(new Section(i, stationList[i - 1], stationList[i], splitLine[2]));
                }
                List<Segment> segmentList = ReadFromSegments(path, sectionList);

                return sectionList;
            }
        }

        private List<Segment> ReadFromSegments(string path, List<Section> sectionList)
        {
            using (StreamReader reader = new StreamReader(path + "/Segments.csv"))
            {
                List<string> allLinesText = File.ReadAllLines(path + "/Segments.csv").ToList();

                List<Segment> segmentList = new List<Segment>();

                for (int i = 1; i <= allLinesText.Count; i++)
                {
                    string[] splitLine = Convert.ToString(allLinesText[i - 1]).Split(',').Select(x => x.Trim()).ToArray();

                    segmentList.Add(new Segment(i, new TrainSet(1, splitLine[0]), sectionList[i - 1], splitLine[3], splitLine[4]));
                }

                return segmentList;
            }
        }
    }
}
