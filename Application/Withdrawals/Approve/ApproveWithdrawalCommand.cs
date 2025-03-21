using Application.Common.Exceptions;
using Application.Common.Interfaces;
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

    public ApproveWithdrawalCommandHandler(IAppDbContext context)
    {
        _context = context;
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

        // TODO: Create Voucher for the sanctioned amount
        //var voucher = new Voucher
        //{
        //    VoucherNo = GenerateVoucherNo(),
        //    FinYear = GetCurrentFinancialYear(),
        //    Date = request.SanctionDate,
        //    Narration = $"Loan sanctioned for Member ID: {withdrawal.MemberId}",
        //    Status = VoucherStatus.APPROVED
        //};

        //// Add voucher lines (assuming you have predefined account heads for debit and credit)
        //var debitAccountHeadId = GetDebitAccountHeadId();
        //var creditAccountHeadId = GetCreditAccountHeadId();

        //voucher.AddVoucherLine(new VoucherLine(debitAccountHeadId, request.SanctionedAmt, 0, "Loan Sanctioned"));
        //voucher.AddVoucherLine(new VoucherLine(creditAccountHeadId, 0, request.SanctionedAmt, "Loan Sanctioned"));

        //_context.Vouchers.Add(voucher);


        // TODO : Update Member's account balance

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}

