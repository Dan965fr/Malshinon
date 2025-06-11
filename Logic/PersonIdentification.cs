using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Models;
using Malshinon.DAL;
using Malshinon.Utils;

namespace Malshinon.Logic
{
    internal static class PersonIdentification
    {
        public static Person Identification(PersonDAL dAL)
        {
            Console.WriteLine("Enter your full name (first and last)");
            string fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be empty.");
            }
            string[] nameParts = fullName.Split(' ');
            if (nameParts.Length < 2)
            {
                throw new ArgumentException("Please enter both first and last name.");
            }
            string firstName = nameParts[0];
            string lastName = nameParts[1];
            Person person = dAL.GetFullName(firstName, lastName);
            if (person != null)
            {
                Console.WriteLine($"Welcome {person.FullName}!");
                return person;
            }
            

            string secretCode = SecretCodeGenerator.GenerateSecretCode();
            Person person1 = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                SecretCode = secretCode,
                Type = "reporter", // Default type is "reporter"
                NumReporters = 0,
                NumMentions = 0
            };
            int generatedId = dAL.AddPerson(person1);
            person1.Id = generatedId;
            if (generatedId > 0)
            {
                Console.WriteLine($"Welcome {person1.FullName}! Your secret code is: {secretCode}");
                return person1;
            }
            else
            {
                throw new Exception("Failed to add person. Please try again.");
            }












        }

    }
}
