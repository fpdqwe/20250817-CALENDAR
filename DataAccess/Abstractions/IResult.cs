namespace DataAccess.Abstractions
{
    public interface IResult<T>
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public T? Result { get; }
        public long Duration { get; }
    }
}
