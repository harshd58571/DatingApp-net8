using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entity;
using API.Interface;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenServices(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        // Implementation for creating a token using the user information
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access token from appsettings.json");
        if (tokenKey.Length < 64) throw new Exception("Token key is too short, please use a longer key");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.UserName) }),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
