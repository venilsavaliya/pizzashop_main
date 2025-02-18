using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PizzaShop.Services;

public class JwtServices
{
    private readonly IConfiguration _config;

    public JwtServices(IConfiguration config)
    {
        _config = config;
    }
    public string GenerateJwtToken(string Email, string Role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Email),
            new Claim(ClaimTypes.Role, Role),
        };

        var jwtConfig = _config.GetSection("jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // var token = new JwtSecurityToken(
        //     issuer: jwtConfig["issuer"],
        //     audience: jwtConfig["audience"],
        //     claims: claims,
        //     expires: DateTime.Now.AddHours(1),
        //     signingCredentials: creds
        // );

          var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
            Issuer = jwtConfig["issuer"],
            Audience = jwtConfig["audience"],
            SigningCredentials = creds
        };

        // Create the token handler and generate the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        Console.WriteLine("generated Token: " + tokenHandler.WriteToken(token));

        // Return the generated JWT token
        return tokenHandler.WriteToken(token);

        // return new JwtSecurityTokenHandler().WriteToken(token);
    }
}