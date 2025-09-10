namespace DataAccess.Abstractions
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }
        private ValidationResult(bool isValid, string erroMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = erroMessage;
        }

        public static ValidationResult Valid()
        {
            return new(true);
        }

        public static ValidationResult Invalid(string errorMessage)
        {
            return new(false, errorMessage);
        }
    }
}
