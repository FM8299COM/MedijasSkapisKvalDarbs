using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Static;
using MovieScribe.Data.ViewModels;
using MovieScribe.Models;
using System.Data;

namespace MovieScribe.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DBContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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

        // Method that handles form submission on registration page
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            // Check if the data sent in form is valid
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            // Check if the email address or username already exists
            var existingUser = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress)
                                ?? await _userManager.FindByNameAsync(registerViewModel.UserName);

            // If the user already exists, send error message
            if (existingUser != null)
            {
                TempData["Error"] = "This email address or username is already taken";
                return View(registerViewModel);
            }

            // Create new user
            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.UserName
            };

            // Process profile picture upload
            await ProcessImageUploadAsync(registerViewModel.ProfilePicture, newUser);

            // Create new user in the identity system
            var createUserResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            // If user creation is successful...
            if (createUserResult.Succeeded)
            {
                // ...add user to 'User' role and redirect to login
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Login", "Account");
            }

            // If user creation failed, show error message
            TempData["Error"] = "There was an issue with the provided password. Please ensure it meets complexity requirements.";
            return View(registerViewModel);
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
