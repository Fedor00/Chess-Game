
using API.Controllers;
using API.Data;
using API.Interfaces;
using API.Logic;
using API.Repository;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IChessGameRepository, ChessGameRepository>();
            services.AddScoped<IChessGameService, ChessGameService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}