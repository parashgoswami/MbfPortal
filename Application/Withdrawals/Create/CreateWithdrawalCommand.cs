using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Withdrawals.Create;

public class CreateWithdrawalCommand : IRequest<int>
{
    public int MemberId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal AppliedAmt { get; set; }
    public string? Remarks { get; set; }
}

public class CreateWithdrawalValidator : AbstractValidator<CreateWithdrawalCommand>
{
    public CreateWithdrawalValidator()
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

public class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalCommand, int>
{
    private readonly IAppDbContext _context;

    public CreateWithdrawalCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var withdrawal = new Withdrawal
        {
            MemberId = request.MemberId,
            ApplicationDate = request.ApplicationDate,
            AppliedAmt = request.AppliedAmt,
            Status = WithdrawalStatus.NEW,
            Remarks = request.Remarks
        };
        _context.Withdrawals.Add(withdrawal);
        await _context.SaveChangesAsync(cancellationToken);
        return withdrawal.Id;
    }
}