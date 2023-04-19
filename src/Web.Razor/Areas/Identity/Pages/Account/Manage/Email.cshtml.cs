using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Core.Application.DTOs;
using Core.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Razor.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        //private readonly IEmailSender _emailSender;

        public EmailModel(
            IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
            //_emailSender = emailSender;
        }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync()
        {
            var user = await _authService.GetCurrentUserAsync(User);
            Email = user.Email;

            Input = new InputModel
            {
                NewEmail = user.Email,
            };

            IsEmailConfirmed = user.EmailConfirmed;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _authService.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _authService.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }

            if (Input.NewEmail != user.Email)
            {
                TokenResponse response = await _authService.GenerateEmailChangeAsync(User, Input.NewEmail);
                
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { area = "Identity", email = Input.NewEmail, response.Token },
                    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.NewEmail,
                //    "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }
            
            TokenResponse confirmationResponse = await _authService.GenerateEmailConfirmationAsync(User);
            
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", confirmationResponse.Token },
                protocol: Request.Scheme);
            
            //await _emailSender.SendEmailAsync(
            //    email,
            //    "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
