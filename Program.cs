using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Logic;
using Malshinon.Models;
using Malshinon.DAL;
using MySql.Data.MySqlClient;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PersonDAL personDal = new PersonDAL();
            Person reporter = PersonIdentification.Identification(personDal);

            if (reporter == null)
            {
                Console.WriteLine("Failed to identify reporter.");
                return;
            }

            IntelReportManager manager = new IntelReportManager();
            manager.SubmitIntel(reporter);
        }





    }


    
}
