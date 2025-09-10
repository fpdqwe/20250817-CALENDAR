using BLL.Abstractions;

namespace BLL.Utilities.IterationStrategies
{
    public class AnnualStrategy : IEventIterationStrategy
    {
        public DateTime GetFirst(DateTime origin, DateTime after)
        {
            var current = origin;
            while (current < after)
            {
                current = current.AddYears(1);
            }
            return current;
        }

        public DateTime[]? Iterate(DateTime origin, DateTime start, DateTime end)
        {
            var occurrences = new List<DateTime>();
            var current = origin;

            // Для yearly проверяем каждое годовщину в пределах года
            while (current <= end)
            {
                if (current >= start && current <= end)
                {
                    occurrences.Add(current);
                }
                current = current.AddYears(1);
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
                current = current.AddYears(1);
            }

            return occurrences.ToArray();
        }
    }
}
