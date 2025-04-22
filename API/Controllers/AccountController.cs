using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(Datacontext context, ITokenService tokenService) : BaseAPIController
{


    public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto)
    {
        if (await UserExists(registerdto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerdto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
            PasswordSalt = hmac.Key
        };
        context.User.Add(user);
        await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
    {
        var user = await context.User.SingleOrDefaultAsync(x => x.UserName == logindto.Username.ToLower());
        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }
    private async Task<bool> UserExists(string username)
    {
        return await context.User.AnyAsync(x => x.UserName == username.ToLower());
    }
}