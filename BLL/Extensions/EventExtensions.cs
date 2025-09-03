using BLL.Dto;
using Domain.Entities;

namespace BLL.Extensions
{
    public static class EventExtensions
    {
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
    }
}
