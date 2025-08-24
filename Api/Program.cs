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
            builder.Configuration.AddJsonFile("Properties/secrets.json");
            var services = builder.Services;

            services.AddLogging();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
            services.Configure<DbContextOptions>(builder.Configuration.GetSection(nameof(DbContextOptions)));

            services.AddSingleton<IContextManager, ContextManager>();
            services.AddTransient<IUserRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Event>, EventRepository>();
            services.AddTransient<IRepository<EventParticipant>, EventParticipantRepository>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<IHasher, BCryptHasher>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IEventParticipantService, EventParticipantService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
