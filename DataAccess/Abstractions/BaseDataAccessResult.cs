namespace DataAccess.Abstractions
{
    public abstract class BaseDataAccessResult<T>
        : IResult<T>
    {
        protected const string SUCCESS = "Request was executed successfully";
        protected const string FAIL = "Request was executed with no result";
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
        public long Duration { get; set; }
        protected BaseDataAccessResult(bool isSuccess,
            T? result,
            string? exMessage,
            long duration = 0)
        {
            IsSuccess = isSuccess;
            Result = result;
            Message = exMessage;
            Duration = duration;
        }
        protected BaseDataAccessResult(T? result, long duration)
        {
            var isSuccess = result != null;
            var message = isSuccess ? SUCCESS : FAIL;

            IsSuccess = isSuccess;
            Result = result;
            Message = message;
            Duration = duration;
        }
        protected BaseDataAccessResult(Exception ex, long duration)
        {
            IsSuccess = false;
            Result = default;
            Message = ex.Message;
            Duration = duration;
        }
        protected BaseDataAccessResult(long duration)
        {
            IsSuccess = false;
            Result = default;
            Message = "Operation cancelled";
            Duration = duration;
        }
        protected BaseDataAccessResult()
        {

        }
    }
}
