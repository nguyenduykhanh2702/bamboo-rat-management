using FluentValidation;

public class CreateRatValidator : AbstractValidator<CreateRatDto>
{
    public CreateRatValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên dúi không được để trống.")
            .MaximumLength(100).WithMessage("Tên dúi không được vượt quá 100 ký tự.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã code không được để trống.")
            .MaximumLength(100).WithMessage("Tên dúi không được vượt quá 100 ký tự.");
    }
}