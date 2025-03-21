using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Loans.Edit;

public class EditLoanCommand : IRequest
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public string? Remarks { get; set; }
}

public class EditLoanValidator : AbstractValidator<EditLoanCommand>
{
    public EditLoanValidator()
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

public class EditLoanCommandHandler : IRequestHandler<EditLoanCommand>
{
    private readonly IAppDbContext _context;

    public EditLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EditLoanCommand request, CancellationToken cancellationToken)
    {

        // Retrieve the existing loan application entity
        var loanApplication = await _context.Loans.FindAsync(request.Id);

        if (loanApplication == null)
        {
            throw new NotFoundException(nameof(Loan), request.Id);
        }

        // Update the loan application entity with the new values from the request
        loanApplication.MemberId = request.MemberId;
        loanApplication.ApplicationDate = request.ApplicationDate;
        loanApplication.AppliedAmt = request.AppliedAmt;
        loanApplication.Remarks = request.Remarks;

        // Save the changes to the repository
        await _context.SaveChangesAsync(cancellationToken);
    }
}
