using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Withdrawals.Approve;

public class ApproveWithdrawalCommand : IRequest
{
    public int Id { get; set; }
    public decimal SanctionedAmt { get; set; }
    public DateTime SanctionDate { get; set; }
}

public class ApproveWithdrawalValidator : AbstractValidator<ApproveWithdrawalCommand>
{
    public ApproveWithdrawalValidator()
    {
        RuleFor(x => x.SanctionedAmt).GreaterThan(0).WithMessage("Sanctioned amount must be greater than 0.");
        RuleFor(x => x.SanctionDate)
            .NotEmpty().WithMessage("Sanction date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Sanction date cannot be in the future.");
    }
}

public class ApproveWithdrawalCommandHandler : IRequestHandler<ApproveWithdrawalCommand>
{
    private readonly IAppDbContext _context;
    private readonly ITimeService _timeService;
    public ApproveWithdrawalCommandHandler(IAppDbContext context, ITimeService timeService)
    {
        _context = context;
        _timeService = timeService;
    }

    public async Task Handle(ApproveWithdrawalCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing withdrawal application entity
        var withdrawal = await _context.Withdrawals.FindAsync(request.Id);

        if (withdrawal == null)
        {
            throw new NotFoundException(nameof(Withdrawal), request.Id);
        }

        // Update the loan application entity with the new values from the request
        withdrawal.SanctionedAmt = request.SanctionedAmt;
        withdrawal.SanctionDate = request.SanctionDate;
        withdrawal.Status = WithdrawalStatus.APPROVED;

        // Create Voucher for the sanctioned amount
        var finYear = _timeService.GetFinancialYear(request.SanctionDate);
        var voucher = new Voucher
        {
            FinYear = finYear,
            Date = request.SanctionDate,
            Narration = $"Withdrawal sanctioned for Member ID: {withdrawal.MemberId}",
            Status = VoucherStatus.POSTED
        };

        // Add voucher lines with predefined account heads for debit and credit
        var debitAccountHeadId = _context.AccountHeads.Where(a => a.Name == AccountHeadConstants.Withdrawal)
            .Select(a => a.Id)
            .FirstOrDefault();

        var creditAccountHeadId = _context.AccountHeads.Where(a => a.Name == AccountHeadConstants.Bank)
            .Select(a => a.Id)
            .FirstOrDefault();

        voucher.AddVoucherLine(new VoucherLine(debitAccountHeadId, request.SanctionedAmt, 0, "Withdrawal by Member"));
        voucher.AddVoucherLine(new VoucherLine(creditAccountHeadId, 0, request.SanctionedAmt, "Withdrawal by Member"));

        if (voucher.IsBalanced())
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync(cancellationToken); // Save the changes to the repository
        }

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}

