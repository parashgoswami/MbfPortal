using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Withdrawals.Edit;

public class EditWithdrawalCommand : IRequest
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public string? Remarks { get; set; }
}

public class EditWithdrawalValidator : AbstractValidator<EditWithdrawalCommand>
{
    public EditWithdrawalValidator()
    {
        RuleFor(x => x.MemberId)
            .GreaterThan(0).WithMessage("Please select a member");

        RuleFor(x => x.ApplicationDate)
            .NotEmpty().WithMessage("Application date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Application date cannot be in the future.");

        RuleFor(x => x.AppliedAmt)
            .GreaterThan(0).WithMessage("Applied amount must be greater than 0.");

        RuleFor(x => x.Remarks)
            .MaximumLength(EntityConstants.RemarksLength).WithMessage($"Remarks cannot exceed {EntityConstants.RemarksLength} character");
    }
}

public class EditWithdrawalCommandHandler : IRequestHandler<EditWithdrawalCommand>
{
    private readonly IAppDbContext _context;

    public EditWithdrawalCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditWithdrawalCommand request, CancellationToken cancellationToken)
    {

        // Retrieve the existing loan application entity
        var withdrawal = await _context.Withdrawals.FindAsync(request.Id);

        if (withdrawal == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        // Update the loan application entity with the new values from the request
        withdrawal.MemberId = request.MemberId;
        withdrawal.ApplicationDate = request.ApplicationDate;
        withdrawal.AppliedAmt = request.AppliedAmt;
        withdrawal.Remarks = request.Remarks;

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}
