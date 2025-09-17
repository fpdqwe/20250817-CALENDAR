using Serilog;

namespace Calendar.Tests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected const string SEPARATOR = "═══════════════════════════════════════════════════════════════";
        protected const string THIN_SEPARATOR = "───────────────────────────────────────────────────────────────";
        protected ILogger Logger { get; private set; }
        protected string TestClassName => GetType().Name;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Logger = TestLoggerFactory.CreateLogger();
            Logger.Information(SEPARATOR);
            Logger.Information("Starting test class: {TestClassName}",
                TestClassName);
            Logger.Information("Logging to file: {Path}", TestLoggerFactory.LogFilePath);
            Logger.Information(SEPARATOR);
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            Logger.Information(SEPARATOR);
            Logger.Information("Completed test class: {TestClassName}",
                TestClassName);
            Logger.Information(SEPARATOR);
        }

        [SetUp]
        public virtual void SetUp()
        {
            Logger.Information(THIN_SEPARATOR);
            Logger.Information("Starting test: {TestName}",
                TestContext.CurrentContext.Test.Name);
            Logger.Information(THIN_SEPARATOR);
        }

        [TearDown]
        public virtual void TearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            if (testResult == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                Logger.Information("✓ Test PASSED: {TestName}", TestContext.CurrentContext.Test.Name);
            }
            else if (testResult == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Logger.Error("✗ Test FAILED: {TestName} - {ErrorMessage}",
                    TestContext.CurrentContext.Test.Name,
                    TestContext.CurrentContext.Result.Message);
            }
            else
            {
                Logger.Warning("? Test {Status}: {TestName}", testResult, TestContext.CurrentContext.Test.Name);
            }

            Logger.Information("{Sep}{NewLine}", THIN_SEPARATOR, Environment.NewLine);
        }

        protected void LogInfo(string message)
        {
            Logger.Information(message);
        }

        protected void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        protected void LogWarning(string message)
        {
            Logger.Warning(message);
        }

        protected void LogError(string message)
        {
            Logger.Error(message);
        }

        protected void LogException(Exception ex, string message = null)
        {
            Logger.Error(ex, message ?? "Exception occurred");
        }
    }
}