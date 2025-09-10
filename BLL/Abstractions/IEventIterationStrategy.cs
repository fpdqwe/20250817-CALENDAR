namespace BLL.Abstractions
{
    public interface IEventIterationStrategy
    {
        DateTime[]? Iterate(DateTime origin, DateTime start, DateTime end);
        DateTime[]? Iterate(DateTime origin, DateTime start, int count);
        DateTime GetFirst(DateTime origin, DateTime after);
    }
}
