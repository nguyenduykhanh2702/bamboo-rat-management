public interface IValidationService
{
    Task ValidateAsync<T>(T dto);
}