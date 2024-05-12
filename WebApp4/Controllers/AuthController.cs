using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp4.Abstractions.Services;

namespace WebApp4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userNameOrEmail= "Admin", string password= "Admin12345!")
        {
            var data = await _authService.LoginAsync(userNameOrEmail, password);
            return StatusCode(data.StatusCode, data);
        }
        [HttpPost]
        public async Task<IActionResult> LoginWithRefreshToken(string refreshToken)
        {
            var data = await _authService.LoginWithRefreshTokenAsync(refreshToken);
            return StatusCode(data.StatusCode, data);
        }
        [Authorize(Roles ="Admin,User")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var data = await _authService.ChangePasswordAsync(email, oldPassword, newPassword);
            return StatusCode(data.StatusCode, data);
        }
        [Authorize(Roles ="User")]
        [HttpPut]
        public async Task<IActionResult> LogOut(string userNameOrEmail)
        {
            var data = await _authService.LogOut(userNameOrEmail);
            return StatusCode(data.StatusCode, data);
        }
    }
}
