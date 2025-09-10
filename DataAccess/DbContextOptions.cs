namespace DataAccess
{
    public class DbContextOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public bool UseSensitiveDataLogging { get; set; } = false;
        public bool EnableDetailedErrors { get; set; } = false;
    }
}
