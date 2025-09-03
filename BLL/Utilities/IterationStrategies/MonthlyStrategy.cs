using BLL.Abstractions;

namespace BLL.Utilities.IterationStrategies
{
    public class MonthlyStrategy : IEventIterationStrategy
    {
        public DateTime GetFirst(DateTime origin, DateTime after)
        {
            var current = origin;
            while (current < after)
            {
                current = current.AddMonths(1);
            }
            return current;
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, DateTime end)
        {
            var occurrences = new List<DateTime>();
            var current = origin <= end ? origin : DateTime.MaxValue;

            if (current < start)
            {
                current = GetFirst(origin, start);
            }

            while (current <= end)
            {
                occurrences.Add(current);
                current = current.AddMonths(1);
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
                current = current.AddMonths(1);
            }

            return occurrences.ToArray();
        }
    }
}
