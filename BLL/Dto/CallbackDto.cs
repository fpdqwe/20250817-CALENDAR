namespace BLL.Dto
{
    public class CallbackDto<T>
    {
        /// <summary>
        /// Server response (data transfer object)
        /// </summary>
        public T? Value { get; set; }
        /// <summary>
        /// Server collection response (dto list)
        /// </summary>
        public List<CallbackDto<T>> Values { get; set; } = new();
        public bool IsDataReceived { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public void AddObject(T dto, string? error = null)
        {
            try
            {
                if (dto != null)
                {
                    IsDataReceived = true;
                    Value = dto;
                }
                else
                {
                    if (Value != null) Value = dto;
                    IsDataReceived = false;
                }
            }
            catch (Exception ex)
            {
                IsDataReceived = false;
                error = ex.Message;
            }
            if (error != null)
            {
                ErrorMessage = error;
            }
        }
        public void SetErrorMessage(string message)
        {
            ErrorMessage = message;
            IsDataReceived = false;
        }
    }
}
