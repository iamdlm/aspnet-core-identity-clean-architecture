using System.Text;
using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Razor.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public ConfirmEmailModel(
            IUserService userService,
            IAuthService authService)
        {
            _userService = userService;
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

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            
            var result = await _authService.ConfirmEmailAsync(new ConfirmEmailRequest()
            {
                UserId = userId,
                Token = code
            });

            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            
            return Page();
        }
    }
}
