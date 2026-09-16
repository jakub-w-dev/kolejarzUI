using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KolejarzUI.Lists;
using KolejarzUI.Model;

namespace KolejarzUI
{
    public partial class ConnectionsForm : Form
    {
        private TabPage TabMainPage { get; set; }

        private Label ConnectionListLabel = new Label();

        public static ListBox ConnectionListBox = new ListBox();

        private List<Connection> connections = new List<Connection>();
        public static TrainSetList trainSetList {  get; set; }
        public static SegmentList segmentList { get; set; }
        
        private static Station firstStation;
        private static Station lastStation;
        private static Connection connection;
        public ConnectionsForm(TabPage tab, TrainSetList TSL, SegmentList sL)
        {
            this.TabMainPage = tab;

            trainSetList = TSL;

            segmentList = sL;
 
            ConnectionListLabel.Location = new Point(40, 20);
            ConnectionListLabel.Text = "List of connections";
            ConnectionListLabel.Size = new Size(200, 20);

            ConnectionListBox.Location = new Point(40, 40);
            ConnectionListBox.Name = "trainSetListBox";
            ConnectionListBox.Size = new Size(500, 300);

            List<Connection> connList = completeList();

            TabMainPage.Controls.AddRange(new Control[] { ConnectionListBox, ConnectionListLabel });

            ConnectionListBox.DataSource = connList;
            ConnectionListBox.DisplayMember = "dataString";
            ConnectionListBox.ValueMember = "train";

            InitializeComponent();
        }

        public static List<Connection> completeList()
        {
            List<Connection> list = new List<Connection>();
            trainSetList.trainSets.ForEach((TrainSet train) =>
            {
                if (trainSetList.DoesTrainSetBelongToSegment(train))
                {
                    firstStation = trainSetList.ReturnFirstStation(train);
                    lastStation = trainSetList.ReturnLastStation(train);

                    connection = new Connection(firstStation, lastStation, train);
                    list.Add(connection);
                }

            });
            return list;

        }
    }
}
