using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CDM.HRManagement.Data;

namespace CDM.HRManagement.Pages.HR
{
    public class ViewApplicantsModel : PageModel
    {
        // department -> positions (kept for the AddApplicant modal)
        public static Dictionary<string, List<string>> DepartmentPositions { get; } = new Dictionary<string, List<string>>
        {
            { "Institute of Computing Studies", new List<string> { "Professor", "IT Technician", "Laboratory Assistant" } },
            { "Institute of Business", new List<string> { "Accountant", "Business Professor", "HR Officer" } },
            { "Institute of Education", new List<string> { "Professor", "Guidance Counselor", "Research Coordinator" } }

        };

        [BindProperty]
        public string? GeneratedLink { get; set; }

        public IActionResult OnPostGenerateLink(string applicantId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Onboarding/Form";
            var link = $"{baseUrl}?token={applicantId}";
            GeneratedLink = link;
            TempData["GeneratedLink"] = GeneratedLink;
            return RedirectToPage();
        }

        public List<InMemoryStore.Applicant> Applicants { get; set; } = new List<InMemoryStore.Applicant>();
        public List<string> Departments => DepartmentPositions.Keys.ToList();

        [BindProperty]
        public InMemoryStore.Applicant NewApplicant { get; set; } = new InMemoryStore.Applicant();

        [TempData]
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
            Applicants = InMemoryStore.Applicants.ToList();
        }

        public IActionResult OnPostAddApplicant()
        {
            if (!ModelState.IsValid)
            {
                Applicants = InMemoryStore.Applicants.ToList();
                return Page();
            }

            NewApplicant.Id = Guid.NewGuid().ToString();
            NewApplicant.ApplicantCode = InMemoryStore.GenerateApplicantId();
            NewApplicant.Submitted = DateTime.Now;
            NewApplicant.Status = "Pending Review";
            InMemoryStore.Applicants.Add(NewApplicant);

            SuccessMessage = "Applicant added successfully.";
            return RedirectToPage();
        }

        // Hire an applicant: move from Applicants -> Employees
        public IActionResult OnPostHireApplicant(string applicantId)
        {
            var applicant = InMemoryStore.Applicants.FirstOrDefault(a => a.Id == applicantId);
            if (applicant == null)
            {
                SuccessMessage = "Applicant not found.";
                return RedirectToPage();
            }

            // create employee
            var employee = new InMemoryStore.Employee
            {
                Id = InMemoryStore.GenerateEmployeeId(),
                Name = applicant.Name,
                Position = applicant.Position,
                Department = applicant.Department,
                Email = applicant.Email,
                Phone = applicant.Phone,
                StartDate = DateTime.Now.Date,
                Status = "Active"
            };

            InMemoryStore.Employees.Add(employee);

            // remove from applicants
            InMemoryStore.Applicants.RemoveAll(a => a.Id == applicantId);

            SuccessMessage = $"Hired {employee.Name} — Employee ID {employee.Id}";
            return RedirectToPage("/HR/ViewApplicants");
        }

        // Optional: reject removes applicant or sets status (simple remove)
        public IActionResult OnPostRejectApplicant(string applicantId)
        {
            InMemoryStore.Applicants.RemoveAll(a => a.Id == applicantId);
            SuccessMessage = "Applicant rejected.";
            return RedirectToPage();
        }

        // API endpoint to get positions for a department (left unchanged)
        public JsonResult OnGetPositions(string department)
        {
            if (DepartmentPositions.ContainsKey(department))
            {
                return new JsonResult(DepartmentPositions[department]);
            }
            return new JsonResult(new List<string>());
        }
    }
}
