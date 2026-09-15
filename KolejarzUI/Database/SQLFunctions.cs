using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kolejarz;
using KolejarzUI.Database;

namespace Kolejarz
{
    public abstract class SQLFunctions : DatabaseConnection
    {
        protected string SQLQuery = "";
        public abstract void CreateTable();
        public abstract void DropTable();
        public abstract void FillTable();
    }
}
