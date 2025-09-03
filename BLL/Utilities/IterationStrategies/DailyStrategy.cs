using BLL.Abstractions;

namespace BLL.Utilities.IterationStrategies
{
    public class DailyStrategy : IEventIterationStrategy
    {
        public DateTime GetFirst(DateTime origin, DateTime after)
        {
            throw new NotImplementedException();
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, DateTime end)
        {
            var occurrences = new List<DateTime>();
            var current = origin <= end ? origin : DateTime.MaxValue;

            // Если событие началось до startDate, находим первое вхождение в году
            if (current < start)
            {
                current = start;
            }

            while (current <= end)
            {
                occurrences.Add(current);
                current = current.AddDays(1);
            }

            return occurrences.Count > 0 ? occurrences.ToArray() : null;
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, int count)
        {
            var occurrences = new List<DateTime>();
            var current = origin >= start ? origin : start;
            var generated = 0;

            while (generated < count && current >= origin)
            {
                occurrences.Add(current);
                generated++;
                current = current.AddDays(1);
            }

            return occurrences.Count > 0 ? occurrences.ToArray() : null;
        }
    }
}
