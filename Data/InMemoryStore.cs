using System;
using System.Collections.Generic;

namespace CDM.HRManagement.Data
{
    public static class InMemoryStore
    {
        // Applicants waiting for review
        public static List<Applicant> Applicants { get; } = new List<Applicant>();

        // Hired employees
        public static List<Employee> Employees { get; } = new List<Employee>();

        // Training records
        public static List<Training> Trainings { get; } = new List<Training>();

        // Simple employee id generator
        private static int _employeeCounter = 0;
        public static string GenerateEmployeeId()
        {
            _employeeCounter++;
            return $"CDM-{_employeeCounter:D3}";
        }

        // Simple applicant id generator
        private static int _applicantCounter = 0;
        public static string GenerateApplicantId()
        {
            _applicantCounter++;
            return $"Applicant-{_applicantCounter:D3}";
        }

        public class Applicant
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string ApplicantCode { get; set; } = string.Empty; // Applicant-001
            public string Name { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public DateTime Submitted { get; set; } = DateTime.UtcNow;
            public string Status { get; set; } = "Pending Review";
        }

        public class Employee
        {
            public string Id { get; set; } = string.Empty; // CDM-001
            public string Name { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
            public string Department { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;
            public string Status { get; set; } = "Active";
        }

        public class Training
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Department { get; set; } = string.Empty;
            public string Position { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.UtcNow.Date;
            public int DurationHours { get; set; }
            public string TrainerName { get; set; } = string.Empty;
            public string TrainerPosition { get; set; } = string.Empty;
            public List<string> Participants { get; set; } = new List<string>();
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
}
