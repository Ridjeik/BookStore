using BLL.Models;
using BLL.Services.Interfaces;
using LibraryCW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCW.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.Login(new LoginDto { Email = model.Email, Password = model.Password });

            if(!result.IsSuccess)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction("Index");
            }
            SetCookies(result.Data.Token);
            // If successful, redirect to home page
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterMember(
                new RegisterDto { 
                    Email = model.Email, 
                    Password = model.Password, 
                    Name = model.Name, 
                    PhoneNumber = model.PhoneNumber, 
                    Surname = model.Surname, 
                    UserName = model.Username 
                }, 
                0);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(model);
            }

            // If successful, redirect to home page
            var resultLogin = await _authService.Login(new LoginDto { Email = model.Email, Password = model.Password });
            if (resultLogin.IsSuccess)
                return RedirectToAction("Index", "Book");
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            this.Response.Cookies.Delete(ApiConstants.JwtToken);
            return this.RedirectToAction("Index", "Book");

        }

        private void SetCookies(string jwtToken)
        {
            this.SetJwtTokenCookie(jwtToken);
        }

        private void SetJwtTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                // It doesn't mater what time we will set since we check the expiration time later :)
                Expires = DateTimeOffset.MaxValue
            };

            this.Response.Cookies.Append(ApiConstants.JwtToken, token, cookieOptions);
        }
    }


}
