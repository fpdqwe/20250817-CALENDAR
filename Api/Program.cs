using Api.Extensions;
using BLL;
using BLL.Abstractions;
using BLL.Services;
using BLL.Utilities;
using DataAccess;
using DataAccess.Abstractions;
using DataAccess.Repositories;
using Domain.Entities;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            var services = builder.Services;
            config.AddJsonFile("Properties/secrets.json");

            services.AddLogging();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
            services.Configure<DbContextOptions>(config.GetSection(nameof(DbContextOptions)));

            var jwtOptions = config.GetRequiredSection(nameof(JwtOptions)).Get<JwtOptions>();
            services.AddApiAuthentication(jwtOptions);
            services.AddSingleton<IContextManager, ContextManager>();
            services.AddTransient<IUserRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Event>, EventRepository>();
            services.AddTransient<IRepository<Participant>, ParticipantRepository>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<IHasher, BCryptHasher>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IParticipantService, ParticipantService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
