namespace DataAccess.Abstractions
{
    public abstract class BaseDataAccessResult<T>
        : IDataAccessResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? ExceptionMessage { get; set; }
        public T? Result { get; set; }
        public long Duration { get; set; }
        protected BaseDataAccessResult(bool isSuccess,
            T? result,
            string? exMessage,
            long duration = 0)
        {
            IsSuccess = isSuccess;
            Result = result;
            ExceptionMessage = exMessage;
            Duration = duration;
        }
        protected BaseDataAccessResult()
        {
            IsSuccess = false;
            ExceptionMessage = null;
            Duration = 0;
        }
    }
}
