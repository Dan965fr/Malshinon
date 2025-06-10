using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using MySql.Data.MySqlClient;

namespace Malshinon.DAL
{
    internal class IntelReportDAL
    {
        private readonly string _connStr = "Server=localhost;user=root;database=malshinon;password";
        public IntelReportDAL()
        {
            // Constructor
        }

    }
}
