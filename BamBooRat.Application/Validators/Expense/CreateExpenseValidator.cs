using FluentValidation;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.ExpenseDate).NotEmpty()
       .WithMessage("Ngày ghi nhận chi phí không được để trống.")
       .LessThanOrEqualTo(DateTime.Today)
       .WithMessage("Ngày ghi chi phí không được lớn hơn ngày hiện tại.");

        RuleFor(x => x.Quantity)
        .GreaterThan(0).
        WithMessage("Số lượng phải lớn hơn 0.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Giá phải lớn hơn 0.");

        RuleFor(x => x.ItemName)
            .NotEmpty()
            .WithMessage("Tên danh mục không được bỏ trống.")
            .MaximumLength(255)
            .WithMessage("Tên danh mục không được vượt quá 255 ký tự");

        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Ghi chú không được vượt quá 500 ký tự");

        RuleFor(x => x.Type)
        .IsInEnum()
        .WithMessage("Loại chi phí không hợp lệ.");
    }
}