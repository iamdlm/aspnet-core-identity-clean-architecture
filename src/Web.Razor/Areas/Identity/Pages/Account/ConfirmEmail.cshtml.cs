using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Razor.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IAuthService _authService;

        public ConfirmEmailModel(
            IAuthService authService)
        {
            _authService = authService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var result = await _authService.ConfirmEmailAsync(new EmailConfirmationRequest()
            {
                UserId = userId,
                Token = code
            });
            
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            
            return Page();
        }
    }
}
