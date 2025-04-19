using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;


[ApiController]
[Route("api/[controller]")]
public class UserController(Datacontext context) : ControllerBase
{


    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.User.ToListAsync();
        return users;

    }

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