namespace DataAccess.Abstractions
{
    public interface IContextManager
    {
        public ApplicationDbContext GenerateDatabaseContext();
    }
}
