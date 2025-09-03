namespace BLL.Extensions
{
    public static class IntExtensions
    {
        public static DateTime GetFirstYearDate(this int year)
        {
            return new DateTime(year, 1, 1);
        }
        public static DateTime GetLastYearDate(this int year)
        {
            return new DateTime(year, 12, 31, 23, 59, 59);
        }
    }
}
