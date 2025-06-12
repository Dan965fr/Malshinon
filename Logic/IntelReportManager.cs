using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using Malshinon.DAL;
using MySql.Data.MySqlClient;

namespace Malshinon.Logic
{
    internal class IntelReportManager
    {
        private readonly IntelReportDAL _inteDAL = new IntelReportDAL();
        private readonly PersonDAL _personDAL = new PersonDAL();

        public void SubmitIntel(Person reporter)
        {
            Console.WriteLine("Enter the details of the intel report:");
            string text = Console.ReadLine();

            List<Person> people = new List<Person>();
            Person target = FindTargetInText(text);

            if (target == null)
            {
                Console.WriteLine("No known target mentioned in the report.");
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Report text cannot be empty.");
                return;
            }
            string reportId = Guid.NewGuid().ToString().Substring(0, 8); // Generate a short report ID
            IntelReport report = new IntelReport
            {
                ReporterId = reporter.Id,
                TargetId = target.Id,
                Text = text,
                Timestamp = DateTime.Now
            };
            
         
            SaveIntelReport(report);

            _personDAL.IncrementReports(reporter.Id); // Increment the number of reports for the reporter
            _personDAL.IncrementMentions(target.Id); // Increment the number of mentions for the target person
            _personDAL.UpdateType(target.Id, "suspect"); // Update the target's type to suspect if not already suspect 
            int reporterReports = _personDAL.GetNumReports(reporter.Id);
            double averageTextLength = _inteDAL.GetAverageReportLength(reporter.Id);

            if(reporterReports >= 10 && averageTextLength >= 100)
            {
                _personDAL.UpdateType(reporter.Id, "potential_agent"); // Update reporter's type to agent if they have 10 or more reports and average text length is 100 or more
                Console.WriteLine($"Reporter {reporter.FullName} is now classified as a potential agent.");
            }


            int targetMentions = _personDAL.GetNumMentions(target.Id);
            if (targetMentions >= 20)
            {
                _personDAL.UpdateType(target.Id, "suspect");
                Console.WriteLine($" Potential threat alert: {target.FullName}");// Update target's type to suspect if they have 20 or more mentions
                
            }



            Console.WriteLine($"Intel report submitted successfully with ID: {reportId} for target: {target.FullName}.");
        }
        private Person FindTargetInText(string text)
        {
            List<string> words = text.Split(' ', ',', '.', '!', '?').ToList();
            for (int i = 0; i < words.Count-1; i++)
            {
                string first = words[i];
                string last = words[i + 1];
                Person person = _personDAL.GetFullName(first, last);
                if (person != null)
                {
                    return person; // Return the first matching person found
                }
            }
            return null; // Return null if no matching person is found
        }
        private void SaveIntelReport(IntelReport report)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;user=root;database=malshinon;password=;"))
                {
                    conn.Open();
                    string query = "INSERT INTO intelreports (reporter_id, target_id, text, timestamp) VALUES (@ReporterId, @TargetId, @Text, @Timestamp)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReporterId", report.ReporterId);
                        cmd.Parameters.AddWithValue("@TargetId", report.TargetId);
                        cmd.Parameters.AddWithValue("@Text", report.Text);
                        cmd.Parameters.AddWithValue("@Timestamp", report.Timestamp);
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving intel report: {ex.Message}");

            }

        }
        public void IdentifyDangerousTargets()
        {
            var targets = _personDAL.GetAllTargets();

            Console.WriteLine("\n Dangerous Targets");
            foreach (var target in targets)
            {
                if (target.NumMentions >= 20) 
                {
                    Console.WriteLine($"{target.FirstName} {target.LastName} - Mentions: {target.NumMentions}");
                }
            }

        }

    }
}
