using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kolejarz;
using KolejarzUI.Forms;
using KolejarzUI.Lists;
using KolejarzUI.Model;

namespace KolejarzUI
{
    public partial class SectionsForm : Form
    {
        private static TabPage sectionsTab{  get; set; }

        public static ListBox sectionListBox = new ListBox();

        public static ListBox fromListBox = new ListBox();
        public static ListBox toListBox = new ListBox();

        private TextBox fromSelected = new TextBox();
        private TextBox toSelected = new TextBox();
        private TextBox distance = new TextBox();
        private TextBox stationTextBox = new TextBox();

        private Section selSec;
        private Station selFrom;
        private Station selTo;

        private Button deleteSection = new Button();
        private Button updateSection = new Button();
        private Button addSection = new Button();
        private Button confirm = new Button();

        public static List<Section> sections { get; set; }
        private List<Station> stationsFrom;
        private List<Station> stationsTo;

        private bool isVisible = false;

        private StationList stationList { get; set; }
        private SectionList sectionList { get; set; }
        private SegmentList segmentList { get; set; }

        public SectionsForm(TabPage SectionsTab, StationList statList, SectionList secList, SegmentList segList)
        {
            sectionsTab = SectionsTab;
            stationList = statList;
            sectionList = secList;
            segmentList = segList;

            sections = sectionList.sections;

            stationsFrom = StationsForm.createStationCloneList(stationList.stations);
            stationsTo = StationsForm.createStationCloneList(stationList.stations);

            sectionListBox.DataSource = sections;
            sectionListBox.DisplayMember = "FromToName";
            sectionListBox.Location = new Point(54, 10);
            sectionListBox.Name = "ListBox1";
            sectionListBox.Size = new Size(380, 100);

            fromListBox.DataSource = stationsFrom;
            fromListBox.DisplayMember = "name";
            fromListBox.ValueMember = "address";
            fromListBox.Location = new Point(54, 200);
            fromListBox.Name = "ListBox1";
            fromListBox.Size = new Size(170, 100);

            fromSelected.Location = new Point(54, 320);
            fromSelected.Name = "Select Box";
            fromSelected.Size = new Size(170, 50);

            toListBox.DataSource = stationsTo;
            toListBox.DisplayMember = "name";
            toListBox.ValueMember = "address";
            toListBox.Location = new Point(260, 200);
            toListBox.Name = "Listbox2";
            toListBox.Size = new Size(170, 100);

            toSelected.Location = new Point(260, 320);
            toSelected.Name = "Select Box";
            toSelected.Size = new Size(170, 50);

            confirm.Location = new Point(440, 351);
            confirm.Name = "Confirm";
            confirm.Size = new Size(60, 20);
            confirm.Text = "Confirm";

            distance.Location = new Point(54, 350);
            distance.Name = "Distance";
            distance.PlaceholderText = "Distance";
            distance.Size = new Size(375, 30);

            fromListBox.Visible = false;
            fromSelected.Visible = false;
            toListBox.Visible = false;
            toSelected.Visible = false;
            confirm.Visible = false;
            distance.Visible = false;

            deleteSection.Location = new Point(54, 150);
            deleteSection.Size = new Size(170, 20);
            deleteSection.Text = "Delete section";

            addSection.Location = new Point(260, 150);
            addSection.Size = new Size(170, 20);
            addSection.Text = "Add section";

            addSection.Click += new System.EventHandler(displayAddSectionForm_Click);

            deleteSection.Click += new System.EventHandler(deleteSection_Click);

            fromListBox.SelectedValueChanged += new EventHandler(fromListBox_SelectedValueChanged);

            toListBox.SelectedValueChanged += new EventHandler(toListBox_SelectedValueChanged);

            confirm.Click += new System.EventHandler(addSection_Click);

            sectionsTab.Controls.AddRange(new Control[] { sectionListBox, deleteSection, addSection, fromListBox, toListBox, fromSelected, toSelected, confirm, distance });

            InitializeComponent();
        }

        private void displayAddSectionForm_Click(object sender, EventArgs e)
        {
            if (!isVisible)
            {
                fromListBox.Visible = true;
                toListBox.Visible = true;
                fromSelected.Visible = true;
                toSelected.Visible = true;
                confirm.Visible = true;
                distance.Visible = true;
                isVisible = true;
            } else
            {
                fromListBox.Visible = false;
                toListBox.Visible = false;
                fromSelected.Visible = false;
                toSelected.Visible = false;
                confirm.Visible = false;
                distance.Visible = false;
                isVisible = false;
            }
        }

        private void deleteSection_Click(object sender, EventArgs e)
        {
            if (sectionListBox.SelectedIndex != -1)
            {
                selSec = (Section)sectionListBox.SelectedItem;
                sectionList.DeleteSection(selSec, segmentList);

                updateData(sectionList.sections, segmentList.segments);
            }
        }

        private void addSection_Click(object sender, EventArgs e)
        {
            if (fromListBox.SelectedIndex != -1 && toListBox.SelectedIndex != -1)
            {
                selFrom = (Station)fromListBox.SelectedItem;
                selTo = (Station)toListBox.SelectedItem;
                string distanceInput = distance.Text;
                
                if (selFrom.name != selTo.name)
                {
                    sectionList.AddSection(selFrom, selTo, distanceInput, segmentList);
                    updateData(sectionList.sections, segmentList.segments);
                }
                fromListBox.Visible = false;
                toListBox.Visible = false;
                fromSelected.Visible = false;
                toSelected.Visible = false;
                confirm.Visible = false;
                distance.Visible = false;
                isVisible = false;
            }
        }
        public void toListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (toListBox.SelectedIndex != -1)
            {
                toSelected.Text = toListBox.SelectedValue.ToString();
            }
        }
        
        public static void updateData(List<Section> secList, List<Segment> segList)
        {
            sectionsTab.Controls.AddRange(new Control[] { sectionListBox });
            sectionListBox.DataSource = null;
            sectionListBox.DataSource = secList;
            sectionListBox.DisplayMember = "FromToName";
            sectionListBox.Invalidate();

            SegmentsForm.segmentListBox.DataSource = null;
            SegmentsForm.segmentListBox.DataSource = segList;
            SegmentsForm.segmentListBox.DisplayMember = "trainSetFromToName";
        }

        public void fromListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (fromListBox.SelectedIndex != -1)
            {
                fromSelected.Text = fromListBox.SelectedValue.ToString();
            }
        }
    }
}
