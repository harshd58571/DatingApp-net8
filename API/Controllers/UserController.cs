using API.Controllers;
using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;


public class UserController(Datacontext context) : BaseAPIController
{

[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.User.ToListAsync();
        return users;

    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.User.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

}