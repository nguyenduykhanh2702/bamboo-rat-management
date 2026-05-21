using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _provider;

    public ValidationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task ValidateAsync<T>(T dto)
    {
        var validator = _provider.GetService<IValidator<T>>();

        if (validator == null) return;

        var result = await validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => new ValidationError
            {
                Field = e.PropertyName.ToLower(),
                Message = e.ErrorMessage
            }).ToList();

            throw new ValidationExceptionCustom(errors);
        }
    }
}