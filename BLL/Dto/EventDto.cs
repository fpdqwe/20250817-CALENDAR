using Domain.Entities;
using Domain.Enums;

namespace BLL.Dto
{
    public class EventDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CreatorId { get; set; }
        public DateTime OriginalDate { get; set; }
        public DateTime[]? OccurenceDates { get; set; }
        public int Duration { get; set; } // Duration in 15min intervals
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<EventParticipant>? Participants { get; set; }
    }
}
