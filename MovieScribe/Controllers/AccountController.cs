using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Static;
using MovieScribe.Data.ViewModels;
using MovieScribe.Models;
using System.Data;
using System.Text.Encodings.Web;
using System.Text;

namespace MovieScribe.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DBContext context, IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        // Asynchronous method to process image upload
        private async Task ProcessImageUploadAsync(IFormFile imageUpload, AppUser user)
        {
            // Check if an image has been uploaded
            if (imageUpload != null)
            {
                // Create a memory stream
                using var memoryStream = new MemoryStream();

                // Asynchronously copy the uploaded file to the memory stream
                await imageUpload.CopyToAsync(memoryStream);

                // Convert the memory stream data to an array of bytes and assign it to user's ProfilePictureData property
                user.ProfilePictureData = memoryStream.ToArray();

                // Get the ContentType of the uploaded file (mime type), assign it to user's ProfilePictureMimeType property
                user.ProfilePictureMimeType = imageUpload.ContentType;
            }
            else
            {
                // If no file has been uploaded, remove ProfilePicture from ModelState to prevent model validation errors
                ModelState.Remove("ProfilePicture");
            }
        }

        public async Task<IActionResult> GetProfilePicture(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.ProfilePictureData == null || user.ProfilePictureMimeType == null)
            {
                // TODO: Return a default image or a NotFound result
            }

            return File(user.ProfilePictureData, user.ProfilePictureMimeType);
        }

        // This method handles the GET request for the Login page.
        public IActionResult Login()
        {
            // If the user is already authenticated, redirect them to the Home page.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            // If the user is not authenticated, present them with the login form.
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            // Validate the incoming data from the form.
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the view with the form and validation messages.
                return View(loginViewModel);
            }

            // Try to find the user by the entered email.
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            // If the user exists and the entered password is correct...
            if (user != null && await _userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                if (!user.EmailConfirmed)
                {
                    // If the email has not been confirmed, show a message to the user
                    TempData["Error"] = "You must confirm your email to login.";
                    return View(loginViewModel);
                }

                // Sign in the user.
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, isPersistent: false, lockoutOnFailure: false);

                // If the sign-in process was successful, redirect the user to the Home page.
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If the sign-in process was not successful, show an error message.
            TempData["Error"] = "Invalid credentials. Please try again.";
            return View(loginViewModel);
        }



        // Method that handles registration page request
        public IActionResult Register()
        {
            // Check if user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Redirect authenticated user to Home
                return RedirectToAction("Index", "Home");
            }

            // Create new RegisterViewModel
            var model = new RegisterViewModel();

            // Render the view with the model
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // ModelState.IsValid checks if the model (data sent to this method) meets all validation requirements
            // If it doesn't, it returns the Register view with the model containing validation errors
            if (!ModelState.IsValid) return View(model);

            // The code here creates a new instance of AppUser (which should be a class representing your User)
            // It assigns values from the model to the new user instance
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.EmailAddress,
            };

            // Process profile picture upload if there is one
            if (model.ProfilePicture != null)
            {
                await ProcessImageUploadAsync(model.ProfilePicture, user);
            }
            // The UserManager.CreateAsync method is called to create a new user in the system with a password
            // The result of this operation is stored in 'result'
            var result = await _userManager.CreateAsync(user, model.Password);

            // Here it checks whether the user was created successfully
            if (result.Succeeded)
            {
                // Logs that the new user was created successfully
                _logger.LogInformation("User created a new account with password.");

                // Generates an email confirmation token for the new user
                // This token will be used to verify the user's email address
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // Encode the generated token
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                // Generate a callback URL for confirming the user's email
                // This URL will lead the user to the ConfirmEmail action method of the Account controller
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, code = encodedCode },
                    protocol: Request.Scheme);

                // The EmailSender is used to send an email to the user
                // The email will contain a link that the user can click to confirm their email address
                await _emailSender.SendEmailAsync(model.EmailAddress, "Apstipriniet savu e-pastu",
                    $"Lūdzu, apstipriniet savu kontu <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>klikšķinot šeit</a>.");

                // Check whether the application requires confirmed accounts for sign-in
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    // If a confirmed account is required, it redirects the user to the RegisterConfirmation action method
                    // The user's email is passed along as a parameter
                    return RedirectToAction("RegisterConfirmation", "Account", new { email = model.EmailAddress });
                }
                else
                {
                    // If a confirmed account is not required, it adds the user to the default 'User' role
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    // Sign the user in
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // Redirect the user to the home page
                    return LocalRedirect("~/");
                }
            }

            // If the user creation was not successful, iterate over all the errors and add them to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            // If we got this far, something failed. Redisplay the form with error messages
            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterConfirmation(string email)
        {
            // Assigns the user's email to a ViewBag variable which can be accessed in the View
            ViewBag.Email = email;
            // Returns the RegisterConfirmation view 
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // If either the userId or code are null, it redirects to the Index page
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // Tries to find a user with the provided userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // If no user is found, returns a NotFound result
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // Decodes the code from Base64
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            // Confirms the user's email with the decoded code
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                // If the confirmation fails, throws an exception with the error details
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}': " + result.Errors.Select(e => e.Description).FirstOrDefault());
            }

            // If the confirmation succeeds, redirects to the Home/Index page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string code)
        {
            // If either the userId or code are null, it returns a BadRequest result
            if (userId == null || code == null)
            {
                return BadRequest("A user ID and code must be supplied for password reset.");
            }
            else
            {
                // Creates a new instance of ResetPasswordViewModel with the provided userId and code
                var model = new ResetPasswordViewModel
                {
                    Code = code,
                    Email = userId
                };
                // Returns the ResetPassword view with the model
                return View(model);
            }
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // Checks if the received model is valid
            if (!ModelState.IsValid)
            {
                // If it's not valid, return the ResetPassword view with the current model to display errors
                return View(model);
            }

            // Tries to find a user with the provided email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // If no user is found, redirects to ResetPasswordConfirmation action
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            // Tries to reset the user's password with the provided model
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                // If the password reset is successful, redirects to ResetPasswordConfirmation action
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }

            // If the password reset fails, adds all error messages to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            // Returns the ResetPassword view with the current model to display errors
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            // Checks if the received model is valid
            if (ModelState.IsValid)
            {
                // Tries to find a user with the provided email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is not found or their email is not confirmed, returns the ForgotPasswordConfirmation view
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                // Generates a password reset token for the user
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Generates a password reset link that will be sent to the user
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
                // Sends a password reset email to the user
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Atiestatīt paroli",
                    $"Lūdzu, atiestatiet paroli, noklikšķinot šeit: <a href='{callbackUrl}'>saite</a>");
                // Returns the ForgotPasswordConfirmation view
                return View("ForgotPasswordConfirmation");
            }

            // If the model is not valid, returns the ForgotPassword view with the current model to display errors
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        [Authorize]  // ensure only logged-in users can access this method
        public async Task<IActionResult> UserDetails()
        {
            // retrieve the user
            var user = await _userManager.GetUserAsync(User);

            // handle scenario when the user is not logged in
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // create a ViewModel with the user's details and pass it to the view
            var viewModel = new UserDetailsViewModel
            {
                EmailAddress = user.Email,
                UserName = user.UserName,
                ID = user.Id,
            };

            // return the view, passing in the ViewModel
            return View(viewModel);
        }

        // The `Authorize` attribute ensures that only logged-in users can access this method.
        // This method is designed to handle GET requests and simply returns the ChangePassword View.
        [Authorize]
        public IActionResult ChangePassword()
        {
            // Simply returns the view that will present the user with a form to change their password.
            return View();
        }

        // The `Authorize` attribute ensures that only logged-in users can access this method.
        // This method is designed to handle POST requests that will come from the form submission on the ChangePassword View.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            // ModelState.IsValid checks if the model received from the form submission is valid
            // according to the data annotations defined in the ViewModel.
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the same view to the user with the current model data
                // and validation errors.
                return View(model);
            }

            // Retrieve the current user using the userManager and the User property from the controller base class.
            // The User property represents the current logged-in user.
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // If the user is not found (which shouldn't occur as they are logged in), 
                // redirect them to the Login page.
                return RedirectToAction("Login", "Account");
            }

            // Try to change the user's password. This will validate the current password, and if it is correct,
            // change it to the new password.
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            // Check if the password change was successful.
            if (!changePasswordResult.Succeeded)
            {
                // If the password change failed (e.g., the current password was incorrect), add the errors to the 
                // ModelState and return the same view to the user with the error messages.
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // If the password change was successful, refresh the user's login information in the authentication
            // cookie to ensure it contains the latest security stamp.
            await _signInManager.RefreshSignInAsync(user);
            // Redirect the user to their user details page.
            return RedirectToAction("UserDetails", "Account");
        }

        // The `Authorize` attribute ensures that only logged-in users can access this method.
        // This method handles a POST request to delete a user's account.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            // Retrieve the user that should be deleted.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // If the user is not found, return a 404 Not Found status.
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // Try to delete the user.
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // If the user deletion failed (which should not occur unless there is an issue with the data store),
                // throw an exception to stop the execution and notify the application of the issue.
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            // Sign out the user (as their account no longer exists).
            await _signInManager.SignOutAsync();
            // Redirect the user (who is now signed out) to the homepage.
            return RedirectToAction("Index", "Home");
        }

    }
}
