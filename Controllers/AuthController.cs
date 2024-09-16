using bossabox_backend_challange.Data;
using bossabox_backend_challange.DTOs;
using bossabox_backend_challange.Models;
using bossabox_backend_challange.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bossabox_backend_challange.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : Controller
{
    private readonly VUTTRDbContext _context;
    private readonly AuthService _authService;

    public AuthController(VUTTRDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] AuthDto dto)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(x => x.Name.Equals(dto.Name));
        if (userExists != null)
        {
            return BadRequest(new ResultDto<object> { Error = "Esse usuário já está cadastrado!" });
        }

        var user = new User { Name = dto.Name };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var token = _authService.GenerateJwtToken(user);
        return Ok(new ResultDto<object> { Data = new { user.Id, Token = token } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] AuthDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Name.Equals(dto.Name));
        if (user == null)
        {
            return NotFound(new ResultDto<object> { Error = "Usuário não encontrado" });
        }

        var token = _authService.GenerateJwtToken(user);
        return Ok(new ResultDto<object> { Data = new { user.Id, Token = token } });
    }
}
