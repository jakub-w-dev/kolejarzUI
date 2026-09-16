using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Kolejarz;
using KolejarzUI;
using KolejarzUI.Lists;
using KolejarzUI.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KolejarzUI.Forms
{
    public class StationsForm : Form
    {

        private TabPage tabStations;

        private ListBox listBox1 = new ListBox();

        private Button deleteStation = new Button();
        private Button updateStation = new Button();
        private Button addStation = new Button();
        private Button confirmUpdate = new Button();
        private Button confirmAdd = new Button();

        private Label stationUpdateLabel = new Label();
        private Label stationAddLabel = new Label();

        private TextBox stationUpdateTextBox = new TextBox();
        private TextBox stationAddName = new TextBox();
        private TextBox stationAddAddress = new TextBox();
        private TextBox stationAddLatitude = new TextBox();
        private TextBox stationAddLongitude = new TextBox();    

        private StationList stationList { get; set; }
        private SectionList sectionList { get; set; }
        private SegmentList segmentList { get; set; }

        private List<Station> stationCloneList1 = new List<Station>();
        private List<Station> stationCloneList2 = new List<Station>();

        private Station selStn;

        public StationsForm(TabPage tab, StationList stationList, SectionList sectionList, SegmentList segmentList)
        {
            tabStations = tab;

            this.stationList = stationList;
            this.sectionList = sectionList;
            this.segmentList = segmentList;

            ClientSize = new Size(500, 206);
            Text = "STACJE KOLEJOWE";

            stationUpdateLabel.Location = new Point(14, 150);
            stationUpdateLabel.Name = "label1";
            stationUpdateLabel.Size = new Size(40, 24);
            stationUpdateLabel.Text = "Adres";

            listBox1.Location = new Point(54, 10);
            listBox1.Name = "ListBox1";
            listBox1.Size = new Size(400, 100);

            stationUpdateLabel.Location = new Point(14, 120);
            stationUpdateLabel.Name = "label1";
            stationUpdateLabel.Size = new Size(40, 24);
            stationUpdateLabel.Text = "Adres";

            stationUpdateTextBox.Location = new Point(54, 120);
            stationUpdateTextBox.Name = "textBox1";
            stationUpdateTextBox.Size = new Size(400, 24);

            stationAddName.Location = new Point(54, 200);
            stationAddName.PlaceholderText = "Name:";
            stationAddName.Name = "stationAdd";
            stationAddName.Size = new Size(400, 24);

            stationAddAddress.Location = new Point(54, 260);
            stationAddAddress.PlaceholderText = "Address:";
            stationAddAddress.Name = "stationAdd";
            stationAddAddress.Size = new Size(400, 24);

            stationAddLongitude.Location = new Point(54, 320);
            stationAddLongitude.Name = "stationAdd";
            stationAddLongitude.PlaceholderText = "Longitude: ";
            stationAddLongitude.Size = new Size(400, 24);

            stationAddLatitude.Location = new Point(54, 380);
            stationAddLatitude.Name = "stationAdd";
            stationAddLatitude.PlaceholderText = "Latitude:";
            stationAddLatitude.Size = new Size(400, 24);

            confirmAdd.Location = new Point(54, 420);
            confirmAdd.Text = "Confirm";
            confirmAdd.Size = new Size(400, 24);

            stationUpdateTextBox.Visible = false;
            stationUpdateLabel.Visible = false;

            deleteStation.Location = new Point(54, 150);
            deleteStation.Size = new Size(110, 20);
            deleteStation.Text = "Delete station";

            listBox1.SelectedValueChanged += new EventHandler(ListBox1_SelectedValueChanged);

            deleteStation.Click += new EventHandler(deleteStation_Click);

            updateStation.Location = new Point(199, 150);
            updateStation.Size = new Size(110, 20);
            updateStation.Text = "Update station";

            addStation.Location = new Point(342, 150);
            addStation.Size = new Size(110, 20);
            addStation.Text = "Add station";

            confirmUpdate.Text = "Confirm";
            confirmUpdate.Location = new Point(464, 120);
            confirmUpdate.Size = new Size(70, 20);
            confirmUpdate.Click += new EventHandler(updateStation_Click);

            confirmUpdate.Visible = false;
            confirmAdd.Visible = false;
            stationAddAddress.Visible = false;
            stationAddLatitude.Visible = false;
            stationAddLongitude.Visible = false;
            stationAddName.Visible = false;

            updateStation.Click += new EventHandler(showUpdateForm_Click);

            addStation.Click += new EventHandler(showAddStationForm_Click);

            confirmAdd.Click += new EventHandler(confirmAdd_Click);

            listBox1.DataSource = stationList.stations;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "address";

            tabStations.Controls.AddRange(new Control[] { listBox1, stationUpdateLabel, stationUpdateTextBox, deleteStation, updateStation, addStation, stationAddName, stationAddAddress, stationAddLongitude, stationAddLatitude, confirmAdd });

            listBox1.ClearSelected();
        }

        private void deleteStation_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                selStn = (Station)listBox1.SelectedItem;

                stationList.DeleteStation(selStn, sectionList, segmentList);

                updateData(stationList.stations, sectionList.sections, segmentList.segments);
            }
        }

        private void showUpdateForm_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                stationUpdateTextBox.Visible = true;
                stationUpdateLabel.Visible = true;
                confirmUpdate.Visible = true;

                tabStations.Controls.AddRange(new Control[] { confirmUpdate });
            }
        }
        private void updateStation_Click(object sender, EventArgs e)
        {
            selStn = (Station)listBox1.SelectedItem;
            string name = stationUpdateTextBox.Text;
            stationList.UpdateStation(selStn, name, sectionList, segmentList);
            updateData(stationList.stations, sectionList.sections, segmentList.segments);

        }

        private void updateData(List<Station> stations, List<Section> sections, List<Segment> segments)
        {
            stationCloneList1 = new List<Station>();
            stationCloneList2 = new List<Station>();

            listBox1.DataSource = null;
            listBox1.DataSource = stations;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "address";

            SectionsForm.sectionListBox.DataSource = null;
            SectionsForm.sectionListBox.DataSource = sections;
            SectionsForm.sectionListBox.DisplayMember = "FromToName";

            stationCloneList1 = createStationCloneList(stations);
            stationCloneList2 = createStationCloneList(stations);

            SectionsForm.fromListBox.DataSource = stationCloneList1;
            SectionsForm.toListBox.DataSource = stationCloneList2;

            SegmentsForm.segmentListBox.DataSource = null;
            SegmentsForm.segmentListBox.DataSource = segments;
            SegmentsForm.segmentListBox.DisplayMember = "trainSetFromToName";
        }

        public void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                //stationTextBox.Text = listBox1.SelectedValue.ToString();
                // If we also wanted to get the displayed text we could use
                // the SelectedItem item property:
                // string s = ((Station)ListBox1.SelectedItem).LongName;
            }
        }

        private void showAddStationForm_Click(object sender, EventArgs e)
        {
            confirmAdd.Visible = true;
            stationAddAddress.Visible = true;
            stationAddLatitude.Visible = true;
            stationAddLongitude.Visible = true;
            stationAddName.Visible = true;
        }

        private void confirmAdd_Click(object sender, EventArgs e)
        {
            string name = stationAddName.Text;
            string address = stationAddAddress.Text;
            string longitude = stationAddLongitude.Text;
            string latitude = stationAddLatitude.Text;

            stationList.AddStation(name, address, longitude, latitude, sectionList, segmentList);
            updateData(stationList.stations, sectionList.sections, segmentList.segments);
        }

        public static List<Station> createStationCloneList(List<Station> stations)
        {
            List<Station> cloneList = new List<Station>();
            stations.ForEach((station) =>
            {
                cloneList.Add(new Station(station.id, station.name, station.address, station.longitude, station.latitude));
            });
            return cloneList;
        }
    }
}
