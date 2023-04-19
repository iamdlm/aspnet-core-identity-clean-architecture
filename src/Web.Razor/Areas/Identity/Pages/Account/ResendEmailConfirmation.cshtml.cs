using System.ComponentModel.DataAnnotations;
using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Razor.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        //private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(
            IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.FindByEmailAsync(Input.Email);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return Page();
            }

            TokenResponse response = await _authService.GenerateEmailConfirmationAsync(User);
            
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { response.Token },
                protocol: Request.Scheme);
            
            //await _emailSender.SendEmailAsync(
            //    Input.Email,
            //    "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

            return Page();
        }
    }
}
