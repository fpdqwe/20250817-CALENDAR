using BLL.Dto;
using Domain.Entities;
using Domain.Enums;

namespace BLL.Extensions
{
    public static class EventExtensions
    {
        #region Actual
        public static DateTime[]? GetOccurences(this Event ev, DateTime start, int count)
        {
            var strategy = ev.IterationTime.ToStrategy();
            return strategy.Iterate(ev.Date, start, count);
        }
        public static DateTime[]? GetOccurences(this Event ev, int year)
        {
            var start = year.GetFirstYearDate();
            var end = year.GetLastYearDate();

            var strategy = ev.IterationTime.ToStrategy();
            return strategy.Iterate(ev.Date, start, end);
        }
        public static EventDto ToDto(this Event ev, int year)
        {
            var result = new EventDto
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
                OccurenceDates = ev.GetOccurences(year)
            };
            if (ev.Participants != null)
                result.Participants = ev.Participants.Select(x => x.ToDto()).ToList();
            return result;
        }
        public static EventDto ToDto(this Event ev)
        {
            var result = new EventDto
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
                OccurenceDates = ev.GetOccurences(DateTime.Now.Year),
            };
            if (ev.Participants != null)
                result.Participants = ev.Participants.Select(x => x.ToDto()).ToList();
            return result;
        }
        #endregion
        #region DataTransfers
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
        public static Event ToEntity(this UpdateEventDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            var result = new Event { Id = dto.Id };
            if (dto.Date != null) result.Date = (DateTime)dto.Date;
            if (dto.Duration != null) result.Duration = (int)dto.Duration;
            if (dto.Description != null) result.Description = dto.Description;
            if (dto.Color != null) result.Color = dto.Color;
            if (dto.Name != null) result.Name = dto.Name;
            if (dto.IterationTime != null) result.IterationTime = (IterationTime)dto.IterationTime;
            if (dto.Ico != null) result.Ico = dto.Ico;
            return result;
        }
        public static Event ToEvent(this DeleteDto dto)
        {
            return new Event { Id = dto.Id };
        }
        #endregion
    }
}
