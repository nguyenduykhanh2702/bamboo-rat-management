using System.Data;
using FluentValidation;

public class CreateBreedingValidator : AbstractValidator<CreateBreedingDto>
{
    public CreateBreedingValidator()
    {
        RuleFor(x => x.MaleId)
            .NotEmpty().WithMessage("Mã code của dúi đực không được để trống.");

        RuleFor(x => x.FemaleId)
            .NotEmpty().WithMessage("Mã code của dúi cái không được để trống.");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Mã code của chuồng phối không được để trống.")
            .MaximumLength(500).WithMessage("Ghi chú không được vượt quá 500 ký tự.");
    }
}