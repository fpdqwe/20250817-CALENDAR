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
    }
}
