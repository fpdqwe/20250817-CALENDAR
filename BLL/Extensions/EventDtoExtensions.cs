using BLL.Dto;
using Domain.Entities;

namespace BLL.Extensions
{
    public static class EventDtoExtensions
    {
        public static Event ToEntity(this EventDto dto)
        {
            var result = new Event
            {
                Id = dto.Id,
                Name = dto.Name,
                Date = dto.OriginalDate,
                DateCreated = dto.DateCreated,
                Description = dto.Description,
                Duration = dto.Duration,
                Color = dto.Color,
                CreatorId = dto.CreatorId,
                Ico = dto.Ico,
                IterationTime = dto.IterationTime,
            };
            if (dto.Participants != null)
                result.Participants = dto.Participants.Select(x => x.ToEntity()).ToList();
            return result;
        }
        public static Event ToEntity(this CreateEventDto dto)
        {
            var owner = new Participant()
            {
                Id = dto.CreatorId,
                Role = "Owner",
                User = new User() { Id = dto.CreatorId }
            };
            var result = new Event
            {
                Id = Guid.NewGuid(),
                Date = dto.Date,
                Duration = dto.Duration,
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                Ico = dto.Ico,
                IterationTime = dto.IterationTime,
                DateCreated = dto.DateCreated,
                Participants = new List<Participant>() { owner }
            };
            result.Participants.First().Event = result;
            return result;
        }
    }
}
