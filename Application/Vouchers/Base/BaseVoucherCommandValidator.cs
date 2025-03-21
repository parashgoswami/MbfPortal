using Domain.Constants;
using FluentValidation;

namespace Application.Vouchers.Base;

public class BaseVoucherCommandValidator<T> : AbstractValidator<T> where T : BaseVoucherCommand
{
    public BaseVoucherCommandValidator()
    {
        RuleFor(x => x.VoucherNo)
            .NotEmpty().WithMessage("Voucher number is required.")
            .MaximumLength(EntityConstants.VoucherNoLength).WithMessage("Voucher number must not exceed 50 characters.");

        RuleFor(x => x.FinYear)
            .NotEmpty().WithMessage("Financial year is required.")
            .MaximumLength(EntityConstants.FinYearLength).WithMessage("Financial year must not exceed 10 characters.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.Narration)
            .NotEmpty().WithMessage("Voucher narration is required.")
            .MaximumLength(EntityConstants.NarrationLength).WithMessage($"Narration must not exceed {EntityConstants.NarrationLength} characters.");

        RuleForEach(x => x.VoucherLines).SetValidator(new BaseVoucherLineDtoValidator());
    }
}

public class BaseVoucherLineDtoValidator : AbstractValidator<BaseVoucherLineDto>
{
    public BaseVoucherLineDtoValidator()
    {
        RuleFor(x => x.AccountHeadId)
            .GreaterThan(1).WithMessage("Account head ID is required.");

        RuleFor(x => x.DebitAmt)
            .GreaterThanOrEqualTo(0).WithMessage("Debit amount must be non-negative.");

        RuleFor(x => x.CreditAmt)
            .GreaterThanOrEqualTo(0).WithMessage("Credit amount must be non-negative.");

        RuleFor(x => x.Narration)
            .NotEmpty().WithMessage("Voucher narration is required.")
            .MaximumLength(EntityConstants.NarrationLength).WithMessage($"Narration must not exceed {EntityConstants.NarrationLength} characters.");
    }
}
