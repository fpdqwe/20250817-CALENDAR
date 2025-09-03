using Domain.Abstractions;

namespace Domain.Entities
{
    public class Participant : IEntity
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = "Owner";
        public int WarningTimeOffset { get; set; } = 0;
        public string? Color { get; set; }
        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
