using Auth.web_api_01.Models;
using Auth.web_api_01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.web_api_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterReqModel request)
        {
            var user = await _authService.RegisterAsync(request);
            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("auth")]
        public async Task<IActionResult> Auth()
        {
            var userIdStr = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok("You are authenticated!");
        }

    }
}
