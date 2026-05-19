public class ValidationExceptionCustom : Exception
{
    public List<ValidationError> Errors { get; }

    public ValidationExceptionCustom(List<ValidationError> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}