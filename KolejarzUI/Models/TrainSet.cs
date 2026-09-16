using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KolejarzUI;
using KolejarzUI.Forms;

namespace KolejarzUI.Model
{
    public class TrainSet
    {
        public TrainSet(int id, string shortName)
        {
            this.id = id;
            this.shortName = shortName;
        }

        public static string SQLCreate = @"CREATE TABLE trainset (
            id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
            name VARCHAR(64)
        )";

        public static string SQLDrop = @"DROP TABLE IF EXISTS trainset";

        public int id { get; set; }

        public string shortName { get; set; }

        public List<Carriage> carriages { get; set; }

        public static List<TrainSet> GenerateTrainSets()
        {
            List<TrainSet> trainSets = new List<TrainSet>();

            char[] letterRange = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            int[] numberRange = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            string trainSetName = "";

            Random rnd = new Random();
            int randomNumber = 0;

            trainSets.Add(new TrainSet(1, MainTabForm.segmentsFileInput[0].train.shortName));

            for (int i = 2; i < 13; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (j < 2)
                    {
                        randomNumber = rnd.Next(letterRange.Length - 1);
                        trainSetName += letterRange[randomNumber];
                    }
                    else if (j == 2)
                    {
                        trainSetName += " ";
                    }
                    else
                    {
                        randomNumber = rnd.Next(numberRange.Length - 1);
                        trainSetName += numberRange[randomNumber];
                    }
                }
                trainSets.Add(new TrainSet(i, trainSetName));
                trainSetName = "";
            }
            return trainSets;
        }

    }
}
