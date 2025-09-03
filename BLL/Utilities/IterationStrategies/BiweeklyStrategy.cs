using BLL.Abstractions;

namespace BLL.Utilities.IterationStrategies
{
    public class BiweeklyStrategy : IEventIterationStrategy
    {
        public DateTime GetFirst(DateTime origin, DateTime after)
        {
            var daysDifference = (after - origin).Days;
            var weeksDifference = (int)Math.Ceiling(daysDifference / 14.0);
            return origin.AddDays(weeksDifference * 14);
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, DateTime end)
        {
            var occurrences = new List<DateTime>();
            var current = origin <= end ? origin : DateTime.MaxValue;

            // Если событие началось до startDate, находим первое вхождение в году
            if (current < start)
            {
                current = GetFirst(origin, start);
            }

            while (current <= end)
            {
                occurrences.Add(current);
                current = current.AddDays(14);
            }

            return occurrences.Count > 0 ? occurrences.ToArray() : null;
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, int count)
        {
            var occurrences = new List<DateTime>();
            var current = origin >= start ? origin : GetFirst(origin, start);
            var generated = 0;

            while (generated < count)
            {
                occurrences.Add(current);
                generated++;
                current = current.AddDays(7);
            }

            return occurrences.ToArray();
        }
    }
}
