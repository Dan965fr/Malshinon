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
            PersonDAL dal = new PersonDAL();
            Person person = PersonIdentification.Identification(dal);

            if(person == null)
            {
                Console.WriteLine("Person identification failed.");
                return;
            }





        }


    }
}
