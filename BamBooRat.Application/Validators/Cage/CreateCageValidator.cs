using FluentValidation;

public class CreateCageValidator : AbstractValidator<CreateCageDto>
{
    public CreateCageValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên chuồng không được để trống.")
            .MaximumLength(100).WithMessage("Tên chuồng không được vượt quá 100 ký tự.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Sức chứa phải lớn hơn 0.")
            .LessThanOrEqualTo(100).WithMessage("Sức chứa không được vượt quá 100.");
    }
}