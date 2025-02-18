using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtService
{


    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(string Email, string Role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Email),
            // new Claim(ClaimTypes.Role, Role),
        };

        var jwtConfig = _config.GetSection("jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig["issuer"],
            audience: jwtConfig["audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
