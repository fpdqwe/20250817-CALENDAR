using Calendar.DataAccess;
using Calendar.DataAccess.Abstractions;
using Calendar.DataAccess.Repositories;
using Calendar.Domain.Entities;
using Calendar.Services;
using Calendar.Services.Abstractions;
using Calendar.Services.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Calendar.Api.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services,
            JwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }

        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddSingleton<IContextManager, ContextManager>();
            services.AddTransient<IUserRepository<User>, UserRepository>();
            services.AddTransient<IEventRepository<Event>, EventRepository>();
            services.AddTransient<IRepository<Event>, EventRepository>();
            services.AddTransient<IRepository<Participant>, ParticipantRepository>();
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<IHasher, BCryptHasher>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IParticipantService, ParticipantService>();
        }
    }
}
