using EventPlannerAPI.Dtos;
using EventPlannerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        // Kullanıcı var mı kontrol et
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }

        // Yeni kullanıcı oluştur
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errorMessages });
        }


        // "User" rolünü kullanıcıya atama
        await _userManager.AddToRoleAsync(user, "User");

        // Token üretme
        var token = await _tokenService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }
}

