using System.Text;
using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Razor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        //private readonly IEmailSender _sender;

        public RegisterConfirmationModel(
            IAuthService authService,
            IUserService userService)
        {
            _authService = authService;
            _userService = userService;
            //_sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userService.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                TokenResponse confirmationDto = await _authService.GenerateEmailConfirmationAsync(User);
                
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", confirmationDto.UserId, code = confirmationDto.Token, returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
