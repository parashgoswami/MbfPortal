using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Loans.Create;

public class CreateLoanCommand : IRequest<int>
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public string? Remarks { get; set; }
}

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
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

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, int>
{
    private readonly IAppDbContext _context;

    public CreateLoanCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan
        {
            MemberId = request.MemberId,
            ApplicationDate = request.ApplicationDate,
            AppliedAmt = request.AppliedAmt,
            Status = LoanStatus.NEW,
            Remarks = request.Remarks
        };        
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync(cancellationToken);
        return loan.Id;
    }
}