using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kolejarz;
using KolejarzUI.Model;

namespace KolejarzUI
{
    internal class CarriageList : SQLFunctions
    {
        private List<Carriage> carriages;

        private List<TrainSet> trainSets;

        private string SQLQuery = "";

        public CarriageList(List<Carriage> carriages, List<TrainSet> trainSets)
        {
            this.carriages = carriages;
            this.trainSets = trainSets;
        }

        public override void CreateTable()
        {
            string SQLQuery = Carriage.SQLCreate;
            cmd.CommandText = SQLQuery;
            cmd.ExecuteNonQuery();
        }

        public override void DropTable()
        {
            string SQLQuery = Carriage.SQLDrop;
            cmd.CommandText = SQLQuery;
            cmd.Connection = mySQLConnection;
            cmd.ExecuteNonQuery();
        }

        public override void FillTable()
        {
            Random rnd = new Random();

            int randomNumber = 0;

            foreach (Carriage car in carriages)
            {
                randomNumber = rnd.Next(1, trainSets.Count - 1);
                cmd.Parameters.Clear();
                cmd.CommandText = $"CALL Add_Carriage({randomNumber}, {car.hasEngine}, \"{car.type}\")";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
