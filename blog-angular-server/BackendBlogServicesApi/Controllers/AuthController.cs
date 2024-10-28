using BackendBlogServicesApi.DTOs;
using BackendBlogServicesApi.Entity;
using BackendBlogServicesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendBlogServicesApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Método de inicio de sesión
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            var result = await _authService.LoginAsync(userLoginDto);

            if (!result.IsSuccess)
                return Unauthorized(result.Message);

            return Ok(result.Value);
        }

        // Método de registro
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            var result = await _authService.RegisterAsync(userRegisterDto);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { status = false, code = 400, message = "Token is required." });
            }

            var validationResult = await _authService.ValidateTokenAsync(request.Token);

            if (!validationResult.IsValid)
            {
                return Unauthorized(new { status = false, code = 401, message = validationResult.Message });
            }

            return Ok(new { status = true, code = 200, message = "Token is valid.", expiration = new TokenInfo().ExpirationString, userId = validationResult.UserId });
        }
    }
}
