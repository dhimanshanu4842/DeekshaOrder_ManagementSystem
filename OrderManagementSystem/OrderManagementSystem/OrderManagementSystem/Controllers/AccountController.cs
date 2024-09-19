using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using Serilog;

namespace OrderManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            try
            {
                Log.Debug("Register method started.");

                if (ModelState.IsValid)
                {
                    Log.Debug("Model state is valid.");
                    var user = new IdentityUser { UserName = register.Email, Email = register.Email };
                    user.EmailConfirmed = true;

                    Log.Debug($"Creating user with email: {register.Email}");
                    var result = await _userManager.CreateAsync(user, register.Password);

                    if (result.Succeeded)
                    {
                        Log.Debug("User registration succeeded.");
                        TempData["SuccessMessage"] = "You have successfully registered. Please log in.";
                        return RedirectToAction("Login", "Account");
                    }

                    Log.Debug("User registration failed. Adding errors to ModelState.");
                    foreach (var error in result.Errors)
                    {
                        Log.Error($"Registration error: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    Log.Debug("Model state is not valid.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An exception occurred during registration: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }

            Log.Debug("Returning Register view with model.");
            return View(register);
        }
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            try
            {
                Log.Debug("Login method started.");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Log.Debug($"Return URL provided: {returnUrl}");
                }
                else
                {
                    Log.Debug("No return URL provided.");
                }
                ViewData["ReturnUrl"] = returnUrl;

                Log.Debug("Returning Login view.");
                return View();
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred in the Login GET method: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View();
            }
        }


        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                Log.Debug("=======Login method started.=========");
                if (!ModelState.IsValid)
                {
                    Log.Debug("Model state is invalid.");
                    return View(model);
                }
                Log.Debug($"Attempting to find user with email: {model.Email}");
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    Log.Debug("=======User not found.=========");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
                if (!user.EmailConfirmed)
                {
                    Log.Debug("=========User email is not confirmed.===========");
                    ModelState.AddModelError(string.Empty, "You need to confirm your email before logging in.");
                    return View(model);
                }
                Log.Debug("==========Attempting to sign in the user.========");
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    Log.Debug("==========User successfully signed in.==========");
                    return RedirectToAction("Index", "Orders");
                }
                else
                {
                    Log.Debug("=======Invalid login attempt.===========");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"====An error occurred during the login process: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Log.Debug("======Logout process started.========");
                await _signInManager.SignOutAsync();
                Log.Debug("========User successfully signed out.==========");

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during logout: {ex.Message}", ex);
                ModelState.AddModelError(string.Empty, "An error occurred while logging out. Please try again.");
                return RedirectToAction("Login", "Account");
            }
        }



    }
}
