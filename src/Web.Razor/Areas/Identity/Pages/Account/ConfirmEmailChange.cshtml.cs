using System.Text;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Razor.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public ConfirmEmailChangeModel(
            IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string code)
        {
            if (email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var result = await _userService.ChangeEmailAsync(User, email, code);
            
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                return Page();
            }

            await _authService.RefreshSignInAsync(User);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}
