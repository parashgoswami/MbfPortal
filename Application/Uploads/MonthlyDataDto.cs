using Domain.Constants;
using FluentValidation;

namespace Application.Uploads;

public class MonthlyDataDto
{
    public string YearMonth { get; set; } = string.Empty;
    public string EmpNo { get; set; } = string.Empty;
    public decimal Deposit { get; set; }
    public decimal LoanRepay { get; set; }
}

public class MonthlyDataDtoValidator : AbstractValidator<MonthlyDataDto>
{
    public MonthlyDataDtoValidator()
    {
        RuleFor(x => x.EmpNo)
            .NotEmpty().WithMessage("Employee number is required.")
            .Length(EntityConstants.EmpNoLength).WithMessage($"Employee number must be {EntityConstants.EmpNoLength} characters long.");

        RuleFor(x => x.YearMonth)
            .NotEmpty().WithMessage("Employee number is required.")
            .Length(EntityConstants.YearMonthLength).WithMessage($"Employee number must be {EntityConstants.YearMonthLength} characters long.");

        RuleFor(x => x.Deposit)
            .GreaterThanOrEqualTo(0).WithMessage("Deposit amount cannot be negative.");

        RuleFor(x => x.LoanRepay)
            .GreaterThanOrEqualTo(0).WithMessage("Loan repayment amount cannot be negative.");
    }
}