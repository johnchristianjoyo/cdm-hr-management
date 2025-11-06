using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CDM.HRManagement.Data;

namespace CDM.HRManagement.Pages.Onboarding
{
    public class FormModel : PageModel
    {
        public string? Token { get; set; }

        public IActionResult OnGet(string? token)
        {
            // Later: Validate if token is valid and not expired
            if (string.IsNullOrEmpty(token))
            {
                // For now, allow access even without token (for testing)
                Token = "test-token";
            }
            else
            {
                Token = token;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Later: Save data to MongoDB
            // For now, just redirect to thank you page

            return RedirectToPage("/Onboarding/ThankYou");
        }
    }
}
