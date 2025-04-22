using System;
using API.Interface;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;

public static class ApplicationServiceExtention
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services
    , IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<Data.Datacontext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenServices>();

        return services;
    }

}
