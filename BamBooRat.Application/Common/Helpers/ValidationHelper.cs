public static class ValidationHelper
{
    public static void AddError(
        List<ValidationError> errors,
        string field,
        string message)
    {
        errors.Add(new ValidationError
        {
            Field = field,
            Message = message
        });
    }

    public static void ThrowIfAny(List<ValidationError> errors)
    {
        if (errors.Any())
        {
            throw new ValidationExceptionCustom(errors);
        }
    }
}