
using API.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Datacontext (DbContextOptions options): DbContext(options)
    {
        
        
            public DbSet<AppUser> User { get; set; }
        }

        
        }
