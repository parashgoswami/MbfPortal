using Domain.Constants;
using FluentValidation;

namespace Application.Vouchers.Base;

public class BaseVoucherCommandValidator<T> : AbstractValidator<T> where T : BaseVoucherCommand
{
    public BaseVoucherCommandValidator()
    {
        RuleFor(x => x.FinYear)
            .NotEmpty()
                .WithMessage("Financial year is required.")
            .MaximumLength(EntityConstants.FinYearLength)
                .WithMessage($"Financial year must not exceed {EntityConstants.FinYearLength} characters.")
            .Matches(@"^\d{4}-\d{2}$")
                .WithMessage("Financial year must be in the format xxxx-yy (e.g. 2025-26).");

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
