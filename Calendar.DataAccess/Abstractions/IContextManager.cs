namespace Calendar.DataAccess.Abstractions
{
    public interface IContextManager
    {
        public ApplicationDbContext CreateDatabaseContext();
    }
}
