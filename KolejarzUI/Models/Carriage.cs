using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolejarzUI.Model
{
    public class Carriage
    {
        public Carriage(int id, bool hasEngine, string type)
        {
            this.id = id;
            this.hasEngine = hasEngine;
            this.type = type;
        }

        public static string SQLCreate = @"CREATE TABLE carriage(
                                        id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
                                        trainset_id INT,
                                        hasEngine BOOL,
                                        type VARCHAR(64),
                                        CONSTRAINT FOREIGN KEY (trainset_id) REFERENCES trainset(id) 
                                        ON DELETE CASCADE
                                    ) ENGINE=INNODB";

        public static string SQLDrop = "DROP TABLE IF EXISTS carriage";
        public bool hasEngine { get; set; }

        public string type { get; set; }
        public int id { get; set; }
        /*
        public string engineType
        {
            get { return this.type + " - engine"; }
        }
        */

        public static List<Carriage> GenerateCarriages()
        {
            List<Carriage> carriages = new List<Carriage>();

            Random rnd = new Random();

            string[] possibleTypes = { "Conductor Carriage", "Passenger Carriage", "Food Carriage", "Utility Carriage" };

            string carriage = "";
            bool hasEngine = false;

            int randomNumber = rnd.Next(possibleTypes.Length);

            for (int i = 1; i < 31; i++)
            {
                randomNumber = rnd.Next(possibleTypes.Length - 1);

                carriage = possibleTypes[randomNumber];

                if (carriage == "Conductor Carriage")
                {
                    hasEngine = true;
                }
                else
                {
                    hasEngine = false;
                }

                carriages.Add(new Carriage(i, hasEngine, carriage));
            }
            return carriages;
        }
    }
}
