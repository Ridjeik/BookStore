using BLL.Constants;
using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryCW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await authService.Login(loginDto);
            if (result.IsSuccess)
            {
                SetCookies(result.Data.Token);
            }
            return Ok(result);
        }
        [HttpPost("register/member")]
        public async Task<IActionResult> RegisterMember(RegisterDto userDto, decimal balance)
        {
            var result = await authService.RegisterMember(userDto, balance);
            return Ok(result);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee(RegisterDto userDto, decimal salary)
        {
            var result = await authService.RegisterEmployee(userDto, salary);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            this.Response.Cookies.Delete(ApiConstants.JwtToken);
            return this.Ok();

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
