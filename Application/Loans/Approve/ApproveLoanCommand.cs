using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Loans.Commands;

public class ApproveLoanCommand : IRequest
{
    public int Id { get; set; }
    public decimal SanctionedAmt { get; set; }
    public DateTime SanctionDate { get; set; }
}

public class ApproveLoanValidator : AbstractValidator<ApproveLoanCommand>
{
    public ApproveLoanValidator()
    {
        RuleFor(x => x.SanctionedAmt).GreaterThan(0).WithMessage("Sanctioned amount must be greater than 0.");
        RuleFor(x => x.SanctionDate)
            .NotEmpty().WithMessage("Sanction date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Sanction date cannot be in the future.");
    }
}

public class ApproveLoanCommandHandler : IRequestHandler<ApproveLoanCommand>
{
    private readonly IAppDbContext _context;
    private readonly ITimeService _timeService;

    public ApproveLoanCommandHandler(IAppDbContext context, ITimeService timeService)
    {
        _context = context;
        _timeService = timeService;
    }

    public async Task Handle(ApproveLoanCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing loan application entity
        var loanApplication = await _context.Loans.FindAsync(request.Id);

        if (loanApplication == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        // Update the loan application entity with the new values from the request
        loanApplication.SanctionedAmt = request.SanctionedAmt;
        loanApplication.SanctionDate = request.SanctionDate;
        loanApplication.Status = LoanStatus.APPROVED;

        // Create Voucher for the sanctioned amount
        var finYear = _timeService.GetFinancialYear(request.SanctionDate);
        var voucher = new Voucher
        {
            FinYear = finYear,
            Date = request.SanctionDate,
            Narration = $"Loan sanctioned for Member ID: {loanApplication.MemberId}",
            Status = VoucherStatus.POSTED
        };

        // Add voucher lines with predefined account heads for debit and credit
        var debitAccountHeadId = _context.AccountHeads.Where(a => a.Name == AccountHeadConstants.Loan)
            .Select(a => a.Id)
            .FirstOrDefault();

        var creditAccountHeadId = _context.AccountHeads.Where(a => a.Name == AccountHeadConstants.Bank)
            .Select(a => a.Id)
            .FirstOrDefault();

        voucher.AddVoucherLine(new VoucherLine(debitAccountHeadId, request.SanctionedAmt, 0, "Loan Sanctioned"));
        voucher.AddVoucherLine(new VoucherLine(creditAccountHeadId, 0, request.SanctionedAmt, "Loan Sanctioned"));

        if(voucher.IsBalanced())
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync(cancellationToken); // Save the changes to the repository
        }
        
    }
}

