using Microsoft.AspNetCore.Mvc;
using MyRadzenBlazor.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        bool isValid = await _authService.ValidateLoginAsync(request.Username, request.Password);
        if (isValid)
        {
            return Ok();
        }
        else
        {
            return Unauthorized();
        }
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}