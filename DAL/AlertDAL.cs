using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Malshinon.Models;

namespace Malshinon.DAL
{
    internal class AlertDAL
    {
        private readonly string _connStr = "server=localhost;user=root;database=malshinon;password=;";
        public AlertDAL()
        {
             //Constrctor

        }
        public void AddAlert(Alert alert)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connStr))
                {
                    conn.Open();
                    string query = "INSERT INTO alerts(target_Id,start_time,end_time,reason) VALUES (@TargetId,@StartTime,@EndTime,@Reason)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TargetId", alert.TargetId);
                        cmd.Parameters.AddWithValue("@StartTime", alert.StartTime);
                        cmd.Parameters.AddWithValue("@EndTime", alert.EndTime);
                        cmd.Parameters.AddWithValue("@Reason", alert.Reason);
                        cmd.ExecuteNonQuery();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding alert: {ex.Message}");
            }
        }
        public List<Alert> GetAllAlert()
        {
            List<Alert> alerts = new List<Alert>();
            using (MySqlConnection conn = new MySqlConnection(_connStr))
            {
                conn.Open();
                string query = "SELECT * FROM alerts";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alert alert = new Alert
                            {
                                Id = reader.GetInt32("id"),
                                TargetId = reader.GetInt32("target_id"),
                                StartTime = reader.GetDateTime("start_time"),
                                EndTime = reader.GetDateTime("end_time"),
                                Reason = reader.GetString("reason")
                            };
                            alerts.Add(alert);
                        }
                    }
                }
            }
            return alerts;

        }
            


    }
}
