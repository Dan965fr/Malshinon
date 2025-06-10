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
        public IntelReport GetReportById(string reportId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM intel_reports WHERE report_id = @reportId";
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
                                    ReportId = reader.GetString("report_id"),
                                    TargetId = reader.GetString("target_id"),
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

    }
}


