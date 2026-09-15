using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KolejarzUI.Forms;
using KolejarzUI.Lists;
using KolejarzUI.Model;

namespace KolejarzUI
{
    public partial class TrainSetsForm : Form
    {
        private static TabPage tabTrainSets;

        private ListBox trainSetListBox = new ListBox();
        private ListBox carriagesListBox = new ListBox();

        private Button deleteTrainSet = new Button();
        private Button updateTrainSet = new Button();
        private Button addTrainSet = new Button();
        private Button confirmUpdateTrainSet = new Button();
        private Button confirmAddTrainSet = new Button();
        private Button confirmUpdateCarriage = new Button();
        private Button confirmAddCarriage = new Button();
        private Button addCarriage = new Button();
        private Button updateCarriage = new Button();
        private Button deleteCarriage = new Button();

        private Label updateTrainSetLabel = new Label();
        private Label updateCarriageLabel = new Label();
        private Label addTrainSetLabel = new Label();
        private Label addCarriageLabel = new Label();
        private Label carriageLabel = new Label();
        private Label isEngineLabel = new Label();

        private TextBox updateTrainSetTextBox = new TextBox();
        private TextBox addTrainSetTextBox = new TextBox();
        private TextBox addCarriageTextBox = new TextBox();
        private TextBox updateCarriageTextBox = new TextBox();

        public List<Carriage> carriages = new List<Carriage>();
        private List<TrainSet> trainSetsCloneList = new List<TrainSet>();
        private List<Carriage> carriagesCloneList = new List<Carriage>();

        private CheckBox isEngineCheckBox = new CheckBox();

        private Station selStn;
        private TrainSet selTrn;
        private Carriage selCar;

        private TrainSetList trainSetList { get;set; }

        public TrainSetsForm(TabPage tab, TrainSetList trainSetList)
        {

            this.trainSetList = trainSetList;

            trainSetList.ExportTrainSetList();

            trainSetListBox.Location = new Point(54, 10);
            trainSetListBox.Name = "trainSetListBox";
            trainSetListBox.Size = new Size(400, 100);

            trainSetListBox.SelectedIndexChanged += new System.EventHandler(displayCarriageList);

            tabTrainSets = tab;

            updateTrainSetLabel.Location = new Point(14, 120);
            updateTrainSetLabel.Name = "deleteTrainSetLabel";
            updateTrainSetLabel.Size = new Size(40, 40);
            updateTrainSetLabel.Text = "New name: ";

            addTrainSetLabel.Location = new Point(14, 120);
            addTrainSetLabel.Name = "addTrainSetLabel";
            addTrainSetLabel.Size = new Size(40, 24);
            addTrainSetLabel.Text = "Name: ";

            carriageLabel.Location = new Point(54, 200);
            carriageLabel.Name = "carriageLabel";
            carriageLabel.Size = new Size(150, 20);
            carriageLabel.Text = "Aktualnie wybrany pociąg";

            deleteCarriage.Location = new Point(54, 380);
            deleteCarriage.Size = new Size(110, 20);
            deleteCarriage.Text = "Delete carriage";

            updateCarriage.Location = new Point(199, 380);
            updateCarriage.Text = "Update carriage";
            updateCarriage.Size = new Size(110, 20);

            addCarriage.Location = new Point(342, 380);
            addCarriage.Text = "Add carriage";
            addCarriage.Size = new Size(110, 20);

            carriagesListBox.Location = new Point(54, 220);
            carriagesListBox.Name = "carriagesListBox";
            carriagesListBox.Size = new Size(400, 100);

            updateCarriageLabel.Location = new Point(14, 330);
            updateCarriageLabel.Name = "updateCarriageLabel";
            updateCarriageLabel.Size = new Size(40, 40);
            updateCarriageLabel.Text = "New type: ";

            updateCarriageTextBox.Location = new Point(54, 330);
            updateCarriageTextBox.Name = "updateCarriageTextBox";
            updateCarriageTextBox.Size = new Size(400, 100);

            isEngineCheckBox.Location = new Point(54, 355);
            isEngineCheckBox.Name = "isEngineCheckBox";
            isEngineCheckBox.Size = new Size(10, 20);

            isEngineLabel.Location = new Point(67, 355);
            isEngineLabel.Text = "Is engine ?";
            isEngineLabel.Size = new Size(100, 100);

            addCarriageLabel.Location = new Point(14, 330);
            addCarriageLabel.Name = "addCarriageLabel";
            addCarriageLabel.Size = new Size(40, 24);
            addCarriageLabel.Text = "Type: ";

            confirmUpdateCarriage.Location = new Point(464, 331);
            confirmUpdateCarriage.Name = "confirmUpdateCarriage";
            confirmUpdateCarriage.Text = "Confirm";
            confirmUpdateCarriage.Size = new Size(70, 20);

            addCarriageTextBox.Location = new Point(54, 330);
            addCarriageTextBox.Name = "addCarriageTextBox";
            addCarriageTextBox.Size = new Size(400, 100);

            confirmAddCarriage.Location = new Point(464, 331);
            confirmAddCarriage.Name = "confirmAddCarriage";
            confirmAddCarriage.Text = "Confirm";
            confirmAddCarriage.Size = new Size(70, 20);

            updateTrainSetTextBox.Location = new Point(54, 120);
            updateTrainSetTextBox.Name = "deleteTrainSetTextBox";
            updateTrainSetTextBox.Size = new Size(400, 24);

            addTrainSetTextBox.Location = new Point(54, 120);
            addTrainSetTextBox.Name = "addTrainSetTextBox";
            addTrainSetTextBox.Size = new Size(400, 24);

            deleteTrainSet.Location = new Point(54, 150);
            deleteTrainSet.Size = new Size(110, 20);
            deleteTrainSet.Text = "Delete train";

            updateTrainSet.Location = new Point(199, 150);
            updateTrainSet.Size = new Size(110, 20);
            updateTrainSet.Text = "Update name";

            addTrainSet.Location = new Point(342, 150);
            addTrainSet.Size = new Size(110, 20);
            addTrainSet.Text = "Add train";

            confirmUpdateTrainSet.Text = "Confirm";
            confirmUpdateTrainSet.Location = new Point(464, 120);
            confirmUpdateTrainSet.Size = new Size(70, 20);
            confirmUpdateTrainSet.Click += new System.EventHandler(updateTrainSet_Click);

            confirmAddTrainSet.Text = "Confirm";
            confirmAddTrainSet.Location = new Point(464, 120);
            confirmAddTrainSet.Size = new Size(70, 20);

            updateTrainSetTextBox.Visible = false;
            updateTrainSetLabel.Visible = false;
            confirmUpdateTrainSet.Visible = false;
            confirmAddTrainSet.Visible = false;
            addTrainSetTextBox.Visible = false;
            addTrainSetLabel.Visible = false;
            isEngineCheckBox.Visible = false;
            isEngineLabel.Visible = false;

            addCarriageLabel.Visible = false;
            addCarriageTextBox.Visible = false;
            confirmAddCarriage.Visible = false;
            updateCarriageLabel.Visible = false;
            updateCarriageTextBox.Visible = false;
            confirmUpdateCarriage.Visible = false;

            deleteTrainSet.Click += new System.EventHandler(deleteTrainSet_Click);
            updateTrainSet.Click += new System.EventHandler(displayUpdateTrainSetForm);
            addTrainSet.Click += new System.EventHandler(displayAddTrainSetFrom);
            confirmAddTrainSet.Click += new System.EventHandler(addTrainSet_Click);

            addCarriage.Click += new System.EventHandler(displayAddCarriageForm);
            updateCarriage.Click += new System.EventHandler(displayUpdateCarriageForm);
            deleteCarriage.Click += new System.EventHandler(deleteCarriage_Click);
            confirmAddCarriage.Click += new System.EventHandler(addCarriage_Click);
            confirmUpdateCarriage.Click += new System.EventHandler(updateCarriage_Click);

            trainSetListBox.DataSource = trainSetList.trainSets;
            trainSetListBox.DisplayMember = "shortName";
            trainSetListBox.ValueMember = "shortName";

            tabTrainSets.BackgroundImage = Image.FromFile(@"C:/Users/kuba/Desktop/test2/backgroundImage0.jpg");

            tabTrainSets.Controls.AddRange(new Control[] { trainSetListBox, updateTrainSetLabel, updateTrainSetTextBox, deleteTrainSet, updateTrainSet, carriagesListBox, carriageLabel, addTrainSet, confirmUpdateTrainSet, addTrainSetLabel, addTrainSetTextBox, confirmAddTrainSet, deleteCarriage, addCarriageLabel, updateCarriage, addCarriage, addCarriageTextBox, confirmAddCarriage, updateCarriageLabel, updateCarriageTextBox, confirmUpdateCarriage, isEngineCheckBox, isEngineLabel});

            InitializeComponent();
        }

        private void displayCarriageList(object sender, EventArgs e)
        {
            
            if (trainSetListBox.DataSource != null)
            {
                selTrn = (TrainSet)trainSetListBox.SelectedItem;
                this.carriageLabel.Text = selTrn.shortName + " - skład pociągu";
                trainSetList.ExportCarriageList(selTrn);
                carriages = trainSetList.carriages;
                carriagesListBox.DataSource = carriages;
                carriagesListBox.DisplayMember = "type";
            }
            
        }

        private void deleteTrainSet_Click(object sender, EventArgs e)
        {
            selTrn = (TrainSet)trainSetListBox.SelectedItem;
            trainSetList.DeleteTrainSet(selTrn);
            this.UpdateData();
        }

        private void UpdateData()
        {
            trainSetListBox.DataSource = null;
            trainSetListBox.DataSource = trainSetList.trainSets;
            trainSetListBox.DisplayMember = "shortName";
            trainSetListBox.ValueMember = "shortName";

            carriagesListBox.DataSource = null;
            carriagesListBox.DataSource = trainSetList.carriages;
            carriagesListBox.DataSource = carriages;
            carriagesListBox.DisplayMember = "type";

            SegmentsForm.trainSetListBox.DataSource = null;
            SegmentsForm.trainSetListBox.DataSource = trainSetList.trainSets;
            SegmentsForm.trainSetListBox.DisplayMember = "shortName";
            SegmentsForm.trainSetListBox.ValueMember = "shortName";

            ConnectionsForm.trainSetList = trainSetList;
            ConnectionsForm.ConnectionListBox.DataSource = null;
            ConnectionsForm.ConnectionListBox.DataSource = ConnectionsForm.completeList();
            ConnectionsForm.ConnectionListBox.DisplayMember = "dataString";
            ConnectionsForm.ConnectionListBox.ValueMember = "trainName";
        }

        private void displayUpdateTrainSetForm(Object sender, EventArgs e)
        {
            updateTrainSetTextBox.Visible = true;
            updateTrainSetLabel.Visible = true;
            confirmUpdateTrainSet.Visible = true;
            addTrainSetLabel.Visible = false;
            addTrainSetTextBox.Visible = false;
            confirmAddTrainSet.Visible = false;
        }

        private void displayAddTrainSetFrom(Object sender, EventArgs e)
        {
            updateTrainSetTextBox.Visible = false;
            updateTrainSetLabel.Visible = false;
            updateTrainSetLabel.Visible = false;
            confirmUpdateTrainSet.Visible = false;
            addTrainSetLabel.Visible = true;
            addTrainSetTextBox.Visible = true;
            confirmAddTrainSet.Visible = true;
        }

        private void displayAddCarriageForm(Object sender, EventArgs e)
        {
            addCarriageLabel.Visible = true;
            addCarriageTextBox.Visible = true;
            confirmAddCarriage.Visible = true;
            updateCarriageLabel.Visible = false;
            updateCarriageTextBox.Visible = false;
            confirmUpdateCarriage.Visible = false;
            isEngineLabel.Visible = true;
            isEngineCheckBox.Visible = true;
        }

        private void displayUpdateCarriageForm(object sender, EventArgs e)
        {
            addCarriageLabel.Visible = false;
            addCarriageTextBox.Visible = false;
            isEngineLabel.Visible= false;
            isEngineCheckBox.Visible = false;
            confirmAddCarriage.Visible = false;
            updateCarriageLabel.Visible = true;
            updateCarriageTextBox.Visible = true;
            confirmUpdateCarriage.Visible = true;
        }

        private void deleteCarriage_Click(object sender, EventArgs e)
        {
            if (carriagesListBox.SelectedItem != null)
            {
                selTrn = (TrainSet)trainSetListBox.SelectedItem;
                selCar = (Carriage)carriagesListBox.SelectedItem;

                trainSetList.DeleteCarriage(selCar, selTrn);
                UpdateData();
            }
        }

        private void addCarriage_Click(Object sender, EventArgs e)
        {
            selTrn = (TrainSet)trainSetListBox.SelectedItem;
            string type = addCarriageTextBox.Text;
            bool hasEngine = isEngineCheckBox.Checked;

            trainSetList.AddCarriage(selTrn, hasEngine, type);
            UpdateData();
        }

        private void updateCarriage_Click(object sender, EventArgs e)
        {
            selCar = (Carriage)carriagesListBox.SelectedItem;
            selTrn = (TrainSet)trainSetListBox.SelectedItem;

            string newType = updateCarriageTextBox.Text;

            trainSetList.UpdateCarriage(selCar, newType, selTrn);
            UpdateData();

        }

        private void addTrainSet_Click(Object sender, EventArgs e)
        {
            string name = addTrainSetTextBox.Text;
            trainSetList.AddTrainSet(name);
            this.UpdateData();
        }

        private void updateTrainSet_Click(Object sender, EventArgs e)
        {
            selTrn = (TrainSet)trainSetListBox.SelectedItem;
            string name = updateTrainSetTextBox.Text;

            trainSetList.UpdateTrainSet(selTrn, name);
            UpdateData();
        }
    }
}
