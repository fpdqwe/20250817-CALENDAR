namespace DataAccess.Abstractions
{
    public interface IDataAccessResult<T>
    {
        public bool IsSuccess { get; }
        public string? ExceptionMessage { get; }
        public T? Result { get; }
        public long Duration { get; }
    }
}
