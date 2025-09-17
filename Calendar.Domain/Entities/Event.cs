using Calendar.Domain.Abstractions;
using Calendar.Domain.Enums;

namespace Calendar.Domain.Entities
{
    public class Event : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CreatorId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } // Duration in 15min intervals
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<Participant>? Participants { get; set; }
        public User Creator { get; set; }
    }
}
