using FluentValidation;

public class CreateHealthRecordValidator : AbstractValidator<CreateHealthRecordDto>
{
    public CreateHealthRecordValidator()
    {
        RuleFor(x => x.RecordDate)
       .NotEmpty()
       .WithMessage("Ngày ghi nhận không được để trống.")
       .LessThanOrEqualTo(DateTime.Today)
       .WithMessage("Ngày ghi nhận không được ở tương lai");

        RuleFor(x => x.Diagnosis)
        .NotEmpty()
        .WithMessage("Chẩn đoán  không được để trống.");
    }
}