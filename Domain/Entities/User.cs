using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string Password { get; set; } = string.Empty;
        public ZodiacSign Zodiac { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public DateTime DateOfBirth { get; set; }
    }
}
