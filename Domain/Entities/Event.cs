using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class Event : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<EventParticipant>? Participants { get; set; }
    }
}
