using BLL.Abstractions;
using Domain.Entities;
using Domain.Enums;

namespace BLL.Dto
{
    public class CreateEventDto : IDto<Event>
    {
        public DateTime Time { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string? Ico { get; set; }
        public IterationTime IterationTime { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid CreatorId { get; set; }

        public Event ToEntity()
        {
            var owner = new EventParticipant()
            {
                Id = CreatorId,
                Role = "Owner",
                User = new User() { Id = CreatorId }
            };
            var result = new Event
            {
                Id = Guid.NewGuid(),
                Date = Time,
                Duration = Duration,
                Name = Name,
                Description = Description,
                Color = Color,
                Ico = Ico,
                IterationTime = IterationTime,
                DateCreated = DateCreated,
                Participants = new List<EventParticipant>() { owner }
            };
            result.Participants.First().Event = result;
            return result;
        }
    }
}
