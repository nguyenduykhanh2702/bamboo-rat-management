
using FluentValidation;
public class CreateWeightHistoryValidator : AbstractValidator<CreateWeightHistoryDto>
{
    public CreateWeightHistoryValidator()
    {
        RuleFor(x => x.Weight)
        .GreaterThan(0)
        .WithMessage("Cân nặng phải lớn hơn 0kg.")
        .LessThanOrEqualTo(3)
        .WithMessage("Cân nặng không được vượt quá 3kg.");

        RuleFor(x => x.RecordedDate)
            .LessThanOrEqualTo(_ => DateTime.UtcNow)
            .WithMessage("Ngày ghi nhận không được ở tương lai.");

    }
}