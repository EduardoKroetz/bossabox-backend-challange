using bossabox_backend_challange.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace bossabox_backend_challange.Services;

public class AuthService
{

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var claimsIdentity = new ClaimsIdentity(claims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Settings.JwtKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
