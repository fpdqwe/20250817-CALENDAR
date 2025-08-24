using BLL;
using BLL.Abstractions;
using BLL.Services;
using BLL.Utilities;
using DataAccess;
using DataAccess.Abstractions;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("secrets.json");
            var services = builder.Services;
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<JwtOptions>(builder.Configuration);
            services.Configure<DbContextOptions>(builder.Configuration);
            services.AddSingleton<IContextManager, ContextManager>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<IHasher, BCryptHasher>();
            services.AddSingleton<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
