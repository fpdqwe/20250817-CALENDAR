using Calendar.Domain.Enums;
using Calendar.Services.Abstractions;
using Calendar.Services.Utilities.IterationStrategies;

namespace Calendar.Services.Extensions
{
    public static class IterationTimeExtensions
    {
        public static IEventIterationStrategy ToStrategy(this IterationTime enumeration)
        {
            return enumeration switch
            {
                IterationTime.Weekly => new WeeklyStrategy(),
                IterationTime.Biweekly => new BiweeklyStrategy(),
                IterationTime.Monthly => new MonthlyStrategy(),
                IterationTime.Annual => new AnnualStrategy(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
