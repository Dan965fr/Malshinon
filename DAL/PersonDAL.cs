using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Mdels;
using MySql.Data.MySqlClient;

namespace Malshinon.DAL
{
    internal class PersonDAL
    {
        private readonly string _connStr = "Server=localhost;user=root;database=malshinon;password";
        public PersonDAL()
        {
            //constractor
        }
        public Person? GetFullName(string firstName, string lastName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM person WHERE FirstName = @FirstName AND LastName = @LastName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Person
                                {
                                    Id = reader.GetInt32("Id"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName"),
                                    SecretCode = reader.GetString("SecretCode"),
                                    Type = reader.GetString("Type"),
                                    NumReports = reader.GetInt32("NumReports"),
                                    NumMentions = reader.GetInt32("NumMentions")

                                };


                            }

                        }



                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving person by full name: {ex.Message}");
            }
            return null;



        }
        public bool AddPerson(Person person)
        {
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "INSERT INTO people (first_Name,last_Name,SecretCode,type,num_Reports,num_Mentions) VALUES(@FirstName, @LastName, @SecretCode, @Type, @NumReports, @NumMentions)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", person.LastName);
                        cmd.Parameters.AddWithValue("@SecretCode", person.SecretCode);
                        cmd.Parameters.AddWithValue("@Type", person.Type);
                        cmd.Parameters.AddWithValue("@NumReports", person.NumReports);
                        cmd.Parameters.AddWithValue("@NumMentions", person.NumMentions);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Returns true if the insert was successful
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding person: {ex.Message}");
                return false; // Return false if there was an error
            }

        }
        public static string GetSecretCode()
        {
            return Guid.NewGuid().ToString();
        }







    }
}
