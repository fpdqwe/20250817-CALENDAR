using Calendar.Domain.Enums;

namespace Calendar.Services.Dto.Event
{
    public class CreateEventDto
    {
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid CreatorId { get; set; }
    }
}
