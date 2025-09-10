using BLL.Abstractions;
using BLL.Utilities.IterationStrategies;
using Domain.Enums;

namespace BLL.Extensions
{
    public static class IterationTimeExtensions
    {
        public static IEventIterationStrategy ToStrategy(this IterationTime enumeration)
        {
            return enumeration switch
            {
                IterationTime.Daily => new DailyStrategy(),
                IterationTime.Weekly => new WeeklyStrategy(),
                IterationTime.Biweekly => new BiweeklyStrategy(),
                IterationTime.Monthly => new MonthlyStrategy(),
                IterationTime.Annual => new AnnualStrategy(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
