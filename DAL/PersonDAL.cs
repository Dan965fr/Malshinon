using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using MySql.Data.MySqlClient;

namespace Malshinon.DAL
{
    internal class PersonDAL
    {
        private readonly string _connStr = "Server=localhost;user=root;database=malshinon;password=;";
        public PersonDAL()
        {
            //constractor
        }
        public Person GetFullName(string firstName, string lastName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM people WHERE first_name = @FirstName AND last_name = @LastName";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Person
                                {
                                    Id = reader.GetInt32("Id"),
                                    FirstName = reader.GetString("first_name"),
                                    LastName = reader.GetString("last_name"),
                                    SecretCode = reader.GetString("secret_code"),
                                    Type = reader.GetString("type"),
                                    NumReporters = reader.GetInt32("num_reporters"),
                                    NumMentions = reader.GetInt32("num_mentions")

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
        public int AddPerson(Person person)
        {
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    Console.WriteLine("Attempting to add person:");
                    Console.WriteLine($"FirstName: {person.FirstName}");
                    Console.WriteLine($"LastName: {person.LastName}");
                    Console.WriteLine($"SecretCode: {person.SecretCode}");
                    Console.WriteLine($"Type: {person.Type}");
                    Console.WriteLine($"NumReports: {person.NumReporters}");
                    Console.WriteLine($"NumMentions: {person.NumMentions}");
                    string query = "INSERT INTO people (first_name,last_name,secret_code,type,num_reporters,num_mentions) VALUES(@FirstName, @LastName, @SecretCode, @Type, @NumReports, @NumMentions)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", person.LastName);
                        cmd.Parameters.AddWithValue("@SecretCode", person.SecretCode);
                        cmd.Parameters.AddWithValue("@Type", person.Type);
                        cmd.Parameters.AddWithValue("@NumReports", person.NumReporters);
                        cmd.Parameters.AddWithValue("@NumMentions", person.NumMentions);
                        cmd.ExecuteNonQuery();
                        return (int)cmd.LastInsertedId; // Return the ID of the newly added person
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding person: {ex.ToString()}");
                return -1; // Return -1 to indicate failure
            }

        }
        public void IncrementReports(int personId)
        {
            UpdateCounter(personId, "num_reporters");
            
        }
        public void IncrementMentions(int personId)
        {
            UpdateCounter(personId, "num_mentions");
        }


        public int GetNumReports(int personId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT num_reporters FROM people WHERE Id = @PersonId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PersonId", personId);
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving number of reports: {ex.Message}");
                return 0;
            }
        }


        public int GetNumMentions(int personId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT num_mentions FROM people WHERE Id = @PersonId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PersonId", personId);
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving number of mentions: {ex.Message}");
                return 0;
            }
        }


        public void UpdateType(int personId, string newType)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "UPDATE people SET Type = @NewType WHERE Id = @PersonId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NewType", newType);
                        cmd.Parameters.AddWithValue("@PersonId", personId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating type: {ex.Message}");
            }
        }
        private void UpdateCounter(int personId, string columnName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = $"UPDATE people SET {columnName} = {columnName} + 1 WHERE Id = @PersonId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PersonId", personId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating counter: {ex.Message}");
            }
        }









    }
}
