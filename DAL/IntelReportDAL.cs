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
        private readonly string _connStr = "Server=localhost;user=root;database=malshinon;password=;";
        public IntelReportDAL()
        {
            // Constructor

        }
        public IntelReport GetReportById(string reportId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM intelreports WHERE reporter_id = @reporterId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reportId", reportId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new IntelReport
                                {
                                    Id = reader.GetInt32("id"),
                                    ReporterId = reader.GetInt32("reporter_id"),
                                    TargetId = reader.GetInt32("target_id"),
                                    Text = reader.GetString("text"),
                                    Timestamp = reader.GetDateTime("timestamp")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving report: {ex.Message}");
            }
            return null;


        }
        public double GetAverageReportLength(int  reporterId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "Select AVG(CHAR_LENGTH(text))  FROM intelreports WHERE reporter_id = @ReporterId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReporterId", reporterId);
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            return Convert.ToDouble(result);
                        }
                        return 0; // Return 0 if no reports found
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating average report length: {ex.Message}");
                return 0; // Return 0 in case of error
            }
        }


    }
}


