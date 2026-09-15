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
using KolejarzUI.Lists;
using KolejarzUI.Model;
using Kolejarz;

namespace KolejarzUI.Forms
{
    public class SegmentsForm : Form
    {
        public static TabPage segmentsTab = new TabPage();

        public static ListBox segmentListBox = new ListBox(); 
        public static ListBox trainSetListBox = new ListBox();
        public static ListBox sectionListBox = new ListBox();

        private Segment selSeg;
        private Section selSec;
        private TrainSet selTrn;

        private static Button deleteSegment = new Button();
        private static Button addSegment = new Button();
        private static Button confirm = new Button();

        private static TextBox ETA = new TextBox();
        private static TextBox ETD = new TextBox();

        private static Label trainSetLabel = new Label();
        private static Label ETALabel = new Label();
        private static Label ETDLabel = new Label();

        private SegmentList segmentList { get; set; }
        private TrainSetList trainSetList { get; set; }

        public SegmentsForm(TabPage segTab, SectionList sectionList, SegmentList segList, TrainSetList trnList)
        {
            segmentsTab = segTab;
            segmentList = segList;
            trainSetList = trnList;

            segmentListBox.DataSource = segmentList.segments;
            segmentListBox.DisplayMember = "trainSetFromToName";
            segmentListBox.Location = new Point(54, 10);
            segmentListBox.Name = "ListBox1";
            segmentListBox.Size = new Size(380, 100);

            deleteSegment.Location = new Point(54, 150);
            deleteSegment.Size = new Size(170, 20);
            deleteSegment.Text = "Delete segment";

            addSegment.Location = new Point(260, 150);
            addSegment.Size = new Size(170, 20);
            addSegment.Text = "Add segment";

            sectionListBox.DataSource = sectionList.sections;
            sectionListBox.DisplayMember = "FromToName";
            sectionListBox.Location = new Point(54, 206);
            sectionListBox.Name = "ListBox1";
            sectionListBox.Size = new Size(170, 144);

            trainSetLabel.Text = "Train Name: ";
            trainSetLabel.Location = new Point(260, 200);
            trainSetLabel.Name = "TrainSetLabel";
            trainSetLabel.Size = new Size(100, 20);

            ETA.Location = new Point(54, 360);
            ETA.Name = "ETA";
            ETA.PlaceholderText = "ETA";
            ETA.Size = new Size(170, 50);

            ETD.Location = new Point(260, 360);
            ETD.Name = "ETD";
            ETD.PlaceholderText = "ETD";
            ETD.Size = new Size(170, 50);

            trainSetListBox.Location = new Point(260, 206);
            trainSetListBox.Name = "TrainSet: ";
            trainSetListBox.Size = new Size(170, 144);
            trainSetListBox.DataSource = trainSetList.trainSets;
            trainSetListBox.DisplayMember = "shortName";

            confirm.Location = new Point(54, 400);
            confirm.Name = "Confirm";
            confirm.Text = "Confirm";
            confirm.Size = new Size(376, 20);

            sectionListBox.Visible = false;
            trainSetLabel.Visible = false;
            trainSetListBox.Visible = false;
            ETALabel.Visible = false;
            ETA.Visible = false;
            ETDLabel.Visible = false;
            ETD.Visible = false;
            confirm.Visible = false;

            addSegment.Click += new EventHandler(displayAddSegmentForm_Click);
            deleteSegment.Click += new EventHandler(deleteSegment_Click);

            confirm.Click += new EventHandler(addSegment_Click);

            segmentsTab.BackgroundImage = Image.FromFile(@"C:/Users/kuba/Desktop/test2/backgroundImage0.jpg");

            segmentsTab.Controls.AddRange(new Control[] { segmentListBox, deleteSegment, addSegment, sectionListBox, trainSetListBox, trainSetLabel, ETA, ETALabel, ETD, ETDLabel, confirm });
        }

        private void displayAddSegmentForm_Click(object sender, EventArgs e)
        {
            sectionListBox.Visible = true;
            trainSetLabel.Visible = true;
            trainSetListBox.Visible = true;
            ETALabel.Visible = true;
            ETA.Visible = true;
            ETDLabel.Visible = true;
            ETD.Visible = true;
            confirm.Visible = true;
        }

        private void addSegment_Click(object sender, EventArgs e)
        {
            if (sectionListBox.SelectedIndex != -1)
            {
                selSec = (Section)sectionListBox.SelectedItem;
                selTrn = (TrainSet)trainSetListBox.SelectedItem;

                string ETAInput = ETA.Text;
                string ETDInput = ETD.Text;

                segmentList.AddSegment(selSec, selTrn, ETAInput, ETDInput);

                updateData();
            }
        }

        private void deleteSegment_Click(object sender, EventArgs e)
        {
            selSeg = (Segment)segmentListBox.SelectedItem;
            segmentList.DeleteSegment(selSeg);
            updateData();
        }

        private void updateData()
        {
            segmentsTab.Controls.AddRange(new Control[] { segmentListBox });
            segmentListBox.DataSource = null;
            segmentListBox.DataSource = segmentList.segments;
            segmentListBox.DisplayMember = "trainSetFromToName";
            segmentListBox.Invalidate();

            ConnectionsForm.segmentList = segmentList;
            ConnectionsForm.trainSetList = trainSetList;
            ConnectionsForm.ConnectionListBox.DataSource = null;
            ConnectionsForm.ConnectionListBox.DataSource = ConnectionsForm.completeList();
            ConnectionsForm.ConnectionListBox.DisplayMember = "dataString";
            ConnectionsForm.ConnectionListBox.ValueMember = "trainName";

        }
    }
}
