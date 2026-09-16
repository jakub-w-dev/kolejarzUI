using System.Drawing;
//using System.Reflection.Emit;
using System.Windows.Forms;
using Kolejarz;
using KolejarzUI.Database;
using KolejarzUI.Lists;
using KolejarzUI.Model;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KolejarzUI.Forms
{
    public class MainTabForm : Form
    {
        private static string path = "C:/Users/kuba/Desktop/test2"; // path for input stream

        private static Files files = new Files();

        private static Data data = files.ReadFromFiles(path);

        private static DatabaseConnection conn = new DatabaseConnection();

        private static List<Station> stationsFileInput = data.Stations;
        public static List<Section> sectionsFileInput = data.Sections;
        public static List<Segment> segmentsFileInput = data.Segments;
        public static List<TrainSet> generatedTrainSetList = TrainSet.GenerateTrainSets();
        public static List<Carriage> generatedCarriageList = Carriage.GenerateCarriages();

        private StationList stationList = new StationList(stationsFileInput);
        private SectionList sectionList = new SectionList(sectionsFileInput);
        private SegmentList segmentList = new SegmentList(segmentsFileInput, sectionsFileInput);
        private TrainSetList trainSetList = new TrainSetList(generatedCarriageList, generatedTrainSetList);
        private CarriageList carriageList = new CarriageList(generatedCarriageList, generatedTrainSetList);

        private StoredProcedures storedProcedures = new StoredProcedures();

        private TabControl tabMainPage = new TabControl();
        private TabPage tabConnections = new TabPage();
        private TabPage tabStations = new TabPage();
        private TabPage tabSections = new TabPage();
        private TabPage tabSegments = new TabPage();
        private TabPage tabTrainSets = new TabPage();

        private void DisplayTabs()
        {
            conn.ConnectToDB();
            Text = "KOLEJARZ v.1.0";
            ClientSize = new Size(600, 500);

            CreateTables();

            tabStations = new TabPage("Stacje kolejowe");
            tabMainPage.Controls.AddRange(new Control[] { tabStations });

            StationsForm stationsForm = new StationsForm(tabStations, stationList, sectionList, segmentList);

            tabSections = new TabPage("Odcinki między stacjami");
            tabMainPage.Controls.AddRange(new Control[] { tabSections });

            SectionsForm sectionsForm = new SectionsForm(tabSections, stationList, sectionList, segmentList);

            tabSegments = new TabPage("Segmenty połączeń");
            tabMainPage.Controls.AddRange(new Control[] { tabSegments });

            SegmentsForm segmentsForm = new SegmentsForm(tabSegments, sectionList, segmentList, trainSetList);

            tabTrainSets = new TabPage("Pociągi");
            tabMainPage.Controls.AddRange(new Control[] { tabTrainSets });

            TrainSetsForm trainSetsForm = new TrainSetsForm(tabTrainSets, trainSetList);

            Controls.AddRange(new Control[] { tabMainPage });
            tabMainPage.Location = new Point(5, 5);
            tabMainPage.Size = new Size(690, 490);

            tabConnections = new TabPage("Połączenia kolejowe");
            tabMainPage.Controls.AddRange(new Control[] { tabConnections });

            ConnectionsForm mainPageForm = new ConnectionsForm(tabConnections, trainSetList, segmentList);
        }

        private void CreateTables()
        {
            segmentList.DropTable();
            sectionList.DropTable();
            stationList.DropTable();
            carriageList.DropTable();
            trainSetList.DropTable();

            stationList.CreateTable();
            sectionList.CreateTable();
            trainSetList.CreateTable();
            segmentList.CreateTable();
            carriageList.CreateTable();

            storedProcedures.CreateStationStoredProcedures();
            storedProcedures.CreateSectionStoredProcedures();
            storedProcedures.CreateSegmentStoredProcedures();
            storedProcedures.CreateTrainSetStoredProcedures();

            stationList.FillTable();
            sectionList.FillTable();
            trainSetList.FillTable();
            segmentList.FillTable();
            carriageList.FillTable();
        }

        public MainTabForm()
        {
            DisplayTabs();
        }
    }
}
