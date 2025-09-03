using DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class ContextManager : IContextManager
    {
        private readonly string _connectionString;
        private readonly bool _useSensitiveDataLogging;
        private readonly ILogger _logger;
        public ContextManager(IOptions<DbContextOptions> options, ILogger<ContextManager> logger)
        {
            ArgumentNullException.ThrowIfNull(options.Value, nameof(options.Value));
            ArgumentNullException.ThrowIfNullOrEmpty(options.Value.ConnectionString, nameof(options.Value.ConnectionString));
            _connectionString = options.Value.ConnectionString;
            _useSensitiveDataLogging = options.Value.UseSensitiveDataLogging;
            _logger = logger;
        }
        public ApplicationDbContext CreateDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_connectionString)
                .EnableSensitiveDataLogging(_useSensitiveDataLogging)
                .EnableDetailedErrors(true)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options;
            _logger.LogDebug("new ApplicationDbContext class instance was initialized");
            return new ApplicationDbContext(options);
        }
    }
}
