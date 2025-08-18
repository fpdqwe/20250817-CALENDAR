using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventsParticipants { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
