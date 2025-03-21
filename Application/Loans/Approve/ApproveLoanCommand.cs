using Application.Common.Exceptions;
using Application.Common.Interfaces;
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

    public ApproveLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
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

        // TODO: Create Voucher for the sanctioned amount
        //var voucher = new Voucher
        //{
        //    VoucherNo = GenerateVoucherNo(),
        //    FinYear = GetCurrentFinancialYear(),
        //    Date = request.SanctionDate,
        //    Narration = $"Loan sanctioned for Member ID: {loanApplication.MemberId}",
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

