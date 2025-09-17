using Serilog;

namespace Calendar.Tests
{
    public class TestLoggerFactory
    {
        public static string LogFilePath { get; private set; }
        static TestLoggerFactory()
        {
            var logDirectory = Directory.GetCurrentDirectory();
            logDirectory = Path.Combine(logDirectory, "..", "..", "..");
            logDirectory = Path.Combine(logDirectory, "test-logs");
            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);

            LogFilePath = Path.Combine(logDirectory, "test-logs.txt");
        }
        public static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: LogFilePath,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true)
            .WriteTo.Console(
                outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        }
    }
}
