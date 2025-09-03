using BLL.Dto;
using Domain.Entities;

namespace BLL.Extensions
{
    public static class EventDtoExtensions
    {
        public static Event ToEntity(this EventDto dto)
        {
            return new Event
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
                Participants = dto.Participants,
                IterationTime = dto.IterationTime,
            };
        }
        public static EventDto ToDto(this Event ev, int year)
        {
            return new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                OriginalDate = ev.Date,
                DateCreated = ev.DateCreated,
                Description = ev.Description,
                Duration = ev.Duration,
                CreatorId = ev.CreatorId,
                Color = ev.Color,
                Ico = ev.Ico,
                IterationTime = ev.IterationTime,
                Participants = ev.Participants,
                OccurenceDates = ev.GetOccurences(year)
            };
        }
    }
}
